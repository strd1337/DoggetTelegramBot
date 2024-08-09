using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.GetAll.Spouses;
using DoggetTelegramBot.Domain.Common.Enums;
using ErrorOr;
using MediatR;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.DTOs;
using DoggetTelegramBot.Application.Families.Commands.Create;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using TransactionConstants = DoggetTelegramBot.Domain.Common.Constants.Transaction.Constants.Transaction;
using MarriageConstants = DoggetTelegramBot.Domain.Common.Constants.Marriage.Constants.Marriage;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

namespace DoggetTelegramBot.Application.Marriages.Commands.Marry
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

            var transactionResult = await ExecuteServiceFeeAsync(
                spouseIds,
                MarriageConstants.Costs.Marry,
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

            await UpdateSpousesMaritalStatus(spouseIds, cancellationToken);

            var familyResult = await CreateFamilyAsync(spouses, cancellationToken);

            await unitOfWork.GetRepository<Marriage, MarriageId>()
                .AddAsync(marriage, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
                FamilyConstants.Logging.Created(familyResult.FamilyId),
                TelegramEvents.Message,
                LoggerConstants.Colors.Create);

            logger.LogCommon(
                FamilyConstants.Requests.Create(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            logger.LogCommon(
               UserConstants.Logging.UpdatedSuccessfully(request.Spouses),
               TelegramEvents.Message,
               LoggerConstants.Colors.Update);

            logger.LogCommon(
               MarriageConstants.Logging.UpdatedSuccessfully(
                   MarriageId.Create(marriage.MarriageId.Value)),
               TelegramEvents.Message,
               LoggerConstants.Colors.Update);

            logger.LogCommon(
                UserConstants.Requests.UpdateMaritalStatus(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Update);

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
                UserConstants.Requests.GetSpouses(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetSpousesByTelegramIdsQuery query = new(telegramIds);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                UserConstants.Requests.GetSpouses(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return result;
        }

        private async Task UpdateSpousesMaritalStatus(
            List<UserId> spouseIds,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                UserConstants.Requests.UpdateMaritalStatus(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Update);

            UpdateSpousesMaritalStatusCommand command = new(
                spouseIds, MaritalStatus.Married);

            _ = await mediator.Send(command, cancellationToken);
        }

        private async Task<ErrorOr<bool>> ExecuteServiceFeeAsync(
           List<UserId> spouseIds,
           decimal amount,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
                TransactionConstants.Requests.ExecuteServiceFee(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            var transactionResult = await transactionService.ExecuteServiceFeeAsync(
                spouseIds,
                amount,
                cancellationToken);

            logger.LogCommon(
                TransactionConstants.Requests.ExecuteServiceFee(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return transactionResult;
        }

        private async Task<FamilyResult> CreateFamilyAsync(
            List<User> spouses,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                FamilyConstants.Requests.Create(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            List<UserId> spouseIds = spouses
                .Select(s => s.UserId)
                .ToList();

            CreateFamilyCommand command = new(spouseIds);
            var result = await mediator.Send(command, cancellationToken);

            return result.Value;
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
                CacheKeyGenerator.GetSpousesByTelegramIdsQuery(spouseIds),
            ];

            var removalTasks = keys
                .Select(key => cacheService.RemoveAsync(key, cancellationToken));

            await Task.WhenAll(removalTasks);
        }

        private async Task RemoveKeyFromCacheAsync(
            List<long> spouseIds,
            CancellationToken cancellationToken)
        {
            string key = CacheKeyGenerator.GetSpousesByTelegramIdsQuery(spouseIds);
            await cacheService.RemoveAsync(key, cancellationToken);
        }
    }
}
