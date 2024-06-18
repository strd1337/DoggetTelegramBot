using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Inventories.Commands.Create
{
    public sealed class CreateInventoryCommandHandler(
        IUnitOfWork unitOfWork) : ICommandHandler<CreateInventoryCommand, CreateInventoryResult>
    {
        public async Task<ErrorOr<CreateInventoryResult>> Handle(
            CreateInventoryCommand request,
            CancellationToken cancellationToken)
        {
            var inventoryRepository = unitOfWork.GetRepository<Inventory, InventoryId>();

            Inventory inventory = Inventory.Create();

            await inventoryRepository.AddAsync(inventory, cancellationToken);

            return new CreateInventoryResult(InventoryId.Create(inventory.Id.Value));
        }
    }
}
