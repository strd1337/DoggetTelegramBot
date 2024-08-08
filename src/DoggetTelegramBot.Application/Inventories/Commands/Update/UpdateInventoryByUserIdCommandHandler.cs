using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Inventories.Commands.Update
{
    public sealed class UpdateInventoryByUserIdCommandHandler(
        IUnitOfWork unitOfWork) : ICommandHandler<UpdateInventoryByInventoryIdCommand, UpdateInventoryResult>
    {
        public async Task<ErrorOr<UpdateInventoryResult>> Handle(
            UpdateInventoryByInventoryIdCommand request,
            CancellationToken cancellationToken)
        {
            var inventoryRepository = unitOfWork.GetRepository<Inventory, InventoryId>();

            var inventory = await inventoryRepository
                .FirstOrDefaultAsync(
                    i => i.InventoryId == request.InventoryId,
                    cancellationToken);

            if (inventory is null ||
                (inventory.IsDeleted is true && request.IsDeleted is false))
            {
                return Errors.Inventory.NotFound;
            }

            if (request.IsDeleted is true)
            {
                inventory.Restore();
            }

            inventory.Update(request.NewAmount);

            await inventoryRepository.UpdateAsync(inventory);

            return new UpdateInventoryResult();
        }
    }
}
