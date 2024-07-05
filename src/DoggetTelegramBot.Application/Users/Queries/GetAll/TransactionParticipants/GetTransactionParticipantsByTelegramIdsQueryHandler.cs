using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Application.DTOs;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll.TransactionParticipants
{
    public sealed class GetTransactionParticipantsByTelegramIdsQueryHandler
        (
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetTransactionParticipantsByTelegramIdsQuery, GetTransactionParticipantsResult>
    {
        public Task<ErrorOr<GetTransactionParticipantsResult>> Handle(
            GetTransactionParticipantsByTelegramIdsQuery request,
            CancellationToken cancellationToken)
        {
            TransactionParticipantDto? fromUser = null;

            if (request.FromTelegramId is not null)
            {
                fromUser = unitOfWork.GetRepository<User, UserId>()
                   .GetWhere(u => request.FromTelegramId == u.TelegramId && !u.IsDeleted)
                   .Select(u => new TransactionParticipantDto(u.UserId, u.InventoryId))
                   .FirstOrDefault();
            }

            var toUser = unitOfWork.GetRepository<User, UserId>()
                .GetWhere(u => request.ToTelegramId == u.TelegramId && !u.IsDeleted)
                .Select(u => new TransactionParticipantDto(u.UserId, u.InventoryId))
                .FirstOrDefault();

            var telegramIds = request.FromTelegramId is null ?
                    [request.ToTelegramId] :
                    new List<long> { request.FromTelegramId.Value, request.ToTelegramId };

            if ((request.FromTelegramId is not null && fromUser is null) || toUser is null)
            {
                logger.LogCommon(
                   Constants.User.Messages.NotFoundRetrieved(telegramIds),
                   TelegramEvents.Message,
                   Constants.LogColors.GetAll);

                return Task.FromResult<ErrorOr<GetTransactionParticipantsResult>>(Errors.User.SomeNotFound);
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(telegramIds),
                TelegramEvents.Message,
                Constants.LogColors.GetAll);

            return Task.FromResult<ErrorOr<GetTransactionParticipantsResult>>(
                new GetTransactionParticipantsResult(toUser, fromUser));
        }
    }
}
