using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Items.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Items.Commands.Add
{
    public sealed class AddItemsCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : ICommandHandler<AddItemsCommand, AddItemsResult>
    {
        public async Task<ErrorOr<AddItemsResult>> Handle(
            AddItemsCommand request,
            CancellationToken cancellationToken)
        {
            List<Item> itemsToAdd = [];
            foreach (var (amountType, values) in request.Values)
            {
                itemsToAdd.AddRange(values.Select(value => Item.Create(
                    request.ServerName,
                    request.Type,
                    (decimal)amountType * 5.0m, // TO DO: Change the item price 
                    value,
                    amountType)));
            }

            await unitOfWork.GetRepository<Item, ItemId>()
                .AddRangeAsync(itemsToAdd, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
               Constants.Item.Messages.Created(itemsToAdd.Select(i => i.ItemId).ToList()),
               TelegramEvents.Message,
               Constants.LogColors.Create);

            return new AddItemsResult();
        }
    }
}
