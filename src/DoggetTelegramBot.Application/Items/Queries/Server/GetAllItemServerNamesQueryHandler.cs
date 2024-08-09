using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Items.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using ErrorOr;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using ItemConstants = DoggetTelegramBot.Domain.Common.Constants.Item.Constants.Item;

namespace DoggetTelegramBot.Application.Items.Queries.Server
{
    public sealed class GetAllItemServerNamesQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetAllItemServerNamesQuery, ItemServerNamesResult>
    {
        public Task<ErrorOr<ItemServerNamesResult>> Handle(
            GetAllItemServerNamesQuery request,
            CancellationToken cancellationToken)
        {
            List<string> serverNames =
            [
                .. unitOfWork.GetRepository<Item, ItemId>()
                    .GetWhere(i => !i.IsDeleted)
                    .Select(i => i.ServerName)
                    .Distinct()
            ];

            logger.LogCommon(
                ItemConstants.Logging.RetrievedServerNames(serverNames.Count == 0),
                TelegramEvents.Message,
                LoggerConstants.Colors.GetAll);

            return Task.FromResult<ErrorOr<ItemServerNamesResult>>(
                new ItemServerNamesResult(serverNames));
        }
    }
}
