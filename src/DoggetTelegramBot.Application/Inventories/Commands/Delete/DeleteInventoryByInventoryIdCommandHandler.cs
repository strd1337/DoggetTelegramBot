using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using ErrorOr;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using InventoryConstants = DoggetTelegramBot.Domain.Common.Constants.Inventory.Constants.Inventory;

namespace DoggetTelegramBot.Application.Inventories.Commands.Delete
{
    public sealed class DeleteInventoryByInventoryIdCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : ICommandHandler<DeleteInventoryByInventoryIdCommand, DeleteInventoryResult>
    {
        public async Task<ErrorOr<DeleteInventoryResult>> Handle(
            DeleteInventoryByInventoryIdCommand request,
            CancellationToken cancellationToken)
        {
            var inventoryRepository = unitOfWork.GetRepository<Inventory, InventoryId>();

            var inventory = await inventoryRepository
                .FirstOrDefaultAsync(
                    i => i.InventoryId == request.InventoryId && !i.IsDeleted,
                    cancellationToken);

            if (inventory is null)
            {
                logger.LogCommon(
                    InventoryConstants.Logging.NotFoundRetrieved(request.InventoryId),
                    TelegramEvents.GroupAction,
                    LoggerConstants.Colors.Get);

                return Errors.Inventory.NotFound;
            }

            logger.LogCommon(
                InventoryConstants.Logging.Retrieved(request.InventoryId),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Get);

            inventory.Update(request.Amount);
            inventory.Delete();

            await inventoryRepository.UpdateAsync(inventory);

            return new DeleteInventoryResult();
        }
    }
}
