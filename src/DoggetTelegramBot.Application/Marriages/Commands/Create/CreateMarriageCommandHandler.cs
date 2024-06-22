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
using DoggetTelegramBot.Domain.Common.Errors;
using PRTelegramBot.Extensions;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using DoggetTelegramBot.Application.Helpers;

namespace DoggetTelegramBot.Application.Marriages.Commands.Create
{
    public sealed class CreateMarriageCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IBotLogger logger,
        IDateTimeProvider dateTimeProvider,
        ICacheService cacheService) : ICommandHandler<CreateMarriageCommand, CreateMarriageResult>
    {
        public async Task<ErrorOr<CreateMarriageResult>> Handle(
            CreateMarriageCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Spouses.Count > 5)
            {
                return Errors.Marriage.TooManySpouses(request.Spouses.Count);
            }

            var result = await GetSpousesByTelegramIds(
                request.Spouses,
                cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            var spouses = result.Value.Spouses;

            if (spouses.Count > 2 && request.Type != MarriageType.Polygamous)
            {
                string typeName = request.Type.GetDescription();

                logger.LogCommon(
                    Constants.Marriage.Messages.WrongType(typeName),
                    TelegramEvents.Message,
                    Constants.LogColors.Create);

                return Errors.Marriage.WrongType(typeName);
            }

            Marriage marriage = Marriage.Create(
                dateTimeProvider.UtcNow,
                request.Type,
                MarriageStatus.Active);

            List<UserId> spouseIds = spouses
                .Select(s => UserId.Create(s.Id.Value))
                .ToList();

            marriage.AddSpouses(spouseIds);

            await UpdateSpousesMaritalStatus(spouses, cancellationToken);

            await unitOfWork.GetRepository<Marriage, MarriageId>()
                .AddAsync(marriage, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await RemoveKeyFromCacheAsync(request.Spouses, cancellationToken);

            return new CreateMarriageResult();
        }

        private async Task<ErrorOr<GetSpousesResult>> GetSpousesByTelegramIds(
            List<long> telegramIds,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.User.Messages.GetSpousesRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GetSpousesByTelegramIdsQuery query = new(telegramIds);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.GetSpousesRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return result;
        }

        private async Task UpdateSpousesMaritalStatus(
            List<User> spouses,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.User.Messages.UpdateMaritalStatusRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            UpdateSpousesMaritalStatusCommand command = new(
                spouses, MaritalStatus.Married);

            _ = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.UpdateMaritalStatusRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);
        }

        private async Task RemoveKeyFromCacheAsync(
            List<long> telegramIds,
            CancellationToken cancellationToken)
        {
            string key = CacheKeyGenerator.GetUsersByTelegramIdsQuery(telegramIds);
            await cacheService.RemoveAsync(key, cancellationToken);
        }
    }
}
