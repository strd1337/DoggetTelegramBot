using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.GetAll.Spouses;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using ErrorOr;
using MediatR;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.DTOs;

namespace DoggetTelegramBot.Application.Marriages.Commands.Create
{
    public sealed class MarryCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IBotLogger logger,
        IDateTimeProvider dateTimeProvider,
        ICacheService cacheService,
        ITransactionService transactionService) : ICommandHandler<MarryCommand, MarriageResult>
    {
        public async Task<ErrorOr<MarriageResult>> Handle(
            MarryCommand request,
            CancellationToken cancellationToken)
        {
            var result = await GetSpousesByTelegramIds(
                request.Spouses,
                cancellationToken);

            if (result.IsError)
            {
                await RemoveKeyFromCacheAsync(request.Spouses, cancellationToken);
                return result.Errors;
            }

            var spouses = result.Value.Spouses;

            List<UserId> spouseIds = spouses
                .Select(s => UserId.Create(s.UserId.Value))
                .ToList();

            var transactionResult = await transactionService.ExecuteServiceFeeAsync(
                spouseIds,
                Constants.Marriage.Costs.Marry,
                cancellationToken);

            if (transactionResult.IsError)
            {
                return transactionResult.Errors;
            }

            Marriage marriage = Marriage.Create(
                dateTimeProvider.UtcNow,
                request.Type,
                MarriageStatus.Active);

            marriage.AddSpouses(spouseIds);

            await UpdateSpousesMaritalStatus(spouses, cancellationToken);

            await unitOfWork.GetRepository<Marriage, MarriageId>()
                .AddAsync(marriage, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
               Constants.User.Messages.UpdatedSuccessfully(request.Spouses),
               TelegramEvents.Message,
               Constants.LogColors.Update);

            logger.LogCommon(
               Constants.Marriage.Messages.UpdatedSuccessfully(
                   MarriageId.Create(marriage.MarriageId.Value)),
               TelegramEvents.Message,
               Constants.LogColors.Update);

            logger.LogCommon(
                Constants.User.Messages.UpdateMaritalStatusRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Update);

            await RemoveKeysFromCacheAsync(spouses, request.Spouses, cancellationToken);

            List<SpouseDto> spousesResult = spouses.
                Select(s => new SpouseDto(
                    s.Username,
                    s.Nickname,
                    s.FirstName))
                .ToList();

            return new MarriageResult(spousesResult);
        }

        private async Task<ErrorOr<GetSpousesResult>> GetSpousesByTelegramIds(
            List<long> telegramIds,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.User.Messages.GetSpousesRequest(),
                TelegramEvents.Message,
                Constants.LogColors.GetAll);

            GetSpousesByTelegramIdsQuery query = new(telegramIds);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.GetSpousesRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.GetAll);

            return result;
        }

        private async Task UpdateSpousesMaritalStatus(
            List<User> spouses,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.User.Messages.UpdateMaritalStatusRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Update);

            UpdateSpousesMaritalStatusCommand command = new(
                spouses, MaritalStatus.Married);

            _ = await mediator.Send(command, cancellationToken);
        }

        private async Task RemoveKeysFromCacheAsync(
            List<User> spouses,
            List<long> spouseIds,
            CancellationToken cancellationToken)
        {
            if (spouses.Count != 2)
            {
                throw new ArgumentException("The list must contain exactly two spouses.");
            }

            var spouseOne = spouses[0];
            var spouseTwo = spouses[1];

            string[] keys =
            [
                CacheKeyGenerator.UserExistsWithTelegramId(spouseOne.TelegramId),
                CacheKeyGenerator.GetUserInfoByTelegramId(spouseOne.TelegramId),
                CacheKeyGenerator.GetFamilyInfoByUserId(UserId.Create(spouseOne.UserId.Value)),
                CacheKeyGenerator.GetAllMarriagesInfoByUserId(UserId.Create(spouseOne.UserId.Value)),
                CacheKeyGenerator.GetInventoryInfoByTelegramId(spouseOne.TelegramId),
                CacheKeyGenerator.UserExistsWithTelegramId(spouseTwo.TelegramId),
                CacheKeyGenerator.GetUserInfoByTelegramId(spouseTwo.TelegramId),
                CacheKeyGenerator.GetFamilyInfoByUserId(UserId.Create(spouseTwo.UserId.Value)),
                CacheKeyGenerator.GetAllMarriagesInfoByUserId(UserId.Create(spouseTwo.UserId.Value)),
                CacheKeyGenerator.GetInventoryInfoByTelegramId(spouseTwo.TelegramId),
                CacheKeyGenerator.GetUsersByTelegramIdsQuery(spouseIds),
            ];

            var removalTasks = keys
                .Select(key => cacheService.RemoveAsync(key, cancellationToken));

            await Task.WhenAll(removalTasks);
        }

        private async Task RemoveKeyFromCacheAsync(
            List<long> spouseIds,
            CancellationToken cancellationToken)
        {
            string key = CacheKeyGenerator.GetUsersByTelegramIdsQuery(spouseIds);
            await cacheService.RemoveAsync(key, cancellationToken);
        }
    }
}
