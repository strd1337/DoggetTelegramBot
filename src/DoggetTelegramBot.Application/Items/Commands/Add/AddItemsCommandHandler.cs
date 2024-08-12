using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Items.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using ErrorOr;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using ItemConstants = DoggetTelegramBot.Domain.Common.Constants.Item.Constants.Item;

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
               ItemConstants.Logging.Created(itemsToAdd.Select(i => i.ItemId).ToList()),
               TelegramEvents.Message,
               LoggerConstants.Colors.Create);

            return new AddItemsResult();
        }
    }
}
