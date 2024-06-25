using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.GetAll.Spouses;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using ErrorOr;
using MediatR;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Application.DTOs;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;

namespace DoggetTelegramBot.Application.Marriages.Commands.Delete
{
    public sealed class DivorceCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IBotLogger logger,
        IDateTimeProvider dateTimeProvider,
        ICacheService cacheService) : ICommandHandler<DivorceCommand, MarriageResult>
    {
        public async Task<ErrorOr<MarriageResult>> Handle(
            DivorceCommand request,
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

            var marriageRepository = unitOfWork.GetRepository<Marriage, MarriageId>();

            var spouses = result.Value.Spouses;

            List<UserId> spouseIds = spouses
                .Select(s => UserId.Create(s.Id.Value))
                .ToList();

            var spouse = spouses.FirstOrDefault();
            var marriage = await marriageRepository
                .FirstOrDefaultAsync(
                    m => m.SpouseIds.Any(id => id.Value == spouse!.Id.Value) &&
                    !m.IsDeleted &&
                    m.Status == MarriageStatus.Active,
                    cancellationToken);

            if (marriage is null)
            {
                logger.LogCommon(
                    Constants.Marriage.Messages.NotFoundRetrieved(spouseIds),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Errors.Marriage.NotFound;
            }

            marriage.Update(dateTimeProvider.UtcNow, MarriageStatus.Divorced);

            await marriageRepository.UpdateAsync(marriage);

            await UpdateSpousesMaritalStatus(spouses, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
               Constants.User.Messages.UpdatedSuccessfully(request.Spouses),
               TelegramEvents.Message,
               Constants.LogColors.Update);

            logger.LogCommon(
              Constants.Marriage.Messages.UpdatedSuccessfully(
                  MarriageId.Create(marriage.Id.Value)),
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

            return new MarriageResult(spousesResult, false);
        }

        private async Task<ErrorOr<GetSpousesResult>> GetSpousesByTelegramIds(
            List<long> telegramIds,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.User.Messages.GetSpousesRequest(),
                TelegramEvents.Message,
                Constants.LogColors.GetAll);

            GetSpousesByTelegramIdsQuery query = new(telegramIds, false);
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
                spouses, MaritalStatus.Divorced);

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
                CacheKeyGenerator.GetFamilyInfoByUserId(UserId.Create(spouseOne.Id.Value)),
                CacheKeyGenerator.GetAllMarriagesInfoByUserId(UserId.Create(spouseOne.Id.Value)),
                CacheKeyGenerator.UserExistsWithTelegramId(spouseTwo.TelegramId),
                CacheKeyGenerator.GetUserInfoByTelegramId(spouseTwo.TelegramId),
                CacheKeyGenerator.GetFamilyInfoByUserId(UserId.Create(spouseTwo.Id.Value)),
                CacheKeyGenerator.GetAllMarriagesInfoByUserId(UserId.Create(spouseTwo.Id.Value)),
                CacheKeyGenerator.GetUsersByTelegramIdsQuery(spouseIds)
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
