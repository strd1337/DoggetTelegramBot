using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.Get;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using ErrorOr;
using MediatR;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;

namespace DoggetTelegramBot.Application.Inventories.Queries.Get.Info
{
    public sealed class GetInventoryInfoQueryByTelegramIdHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        IMediator mediator) : IQueryHandler<GetInventoryInfoByTelegramIdQuery, GetInventoryResult>
    {
        public async Task<ErrorOr<GetInventoryResult>> Handle(
            GetInventoryInfoByTelegramIdQuery request,
            CancellationToken cancellationToken)
        {
            var userResult = await GetUserAsync(request.TelegramId, cancellationToken);

            if (userResult.IsError)
            {
                return userResult.Errors;
            }

            var user = userResult.Value.User;

            var inventory = await unitOfWork.GetRepository<Inventory, InventoryId>()
                .FirstOrDefaultAsync(
                    u => u.InventoryId == user.InventoryId,
                    cancellationToken);

            return inventory is null ?
                Errors.Inventory.NotFound :
                new GetInventoryResult(inventory.YuanBalance);
        }

        private async Task<ErrorOr<GetUserResult>> GetUserAsync(
            long telegramId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                UserConstants.Requests.Get(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetUserByTelegramIdQuery query = new(telegramId);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                UserConstants.Requests.Get(true),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return result;
        }
    }
}
