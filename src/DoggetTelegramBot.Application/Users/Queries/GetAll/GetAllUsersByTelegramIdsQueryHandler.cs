using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll
{
    public sealed class GetAllUsersByTelegramIdsQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetAllUsersByTelegramIdsQuery, GetAllUsersResult>
    {
        public async Task<ErrorOr<GetAllUsersResult>> Handle(
            GetAllUsersByTelegramIdsQuery request,
            CancellationToken cancellationToken)
        {
            List<User> users =
            [
                .. await unitOfWork.GetRepository<User, UserId>()
                    .GetWhereAsync(
                        u => request.TelegramIds.Contains(u.TelegramId) && !u.IsDeleted,
                        cancellationToken)
            ];

            if (users.Count == 0)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramIds),
                    TelegramEvents.Message,
                    Constants.LogColors.GetAll);

                return Errors.User.SomeNotFound;
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramIds),
                TelegramEvents.Message,
                Constants.LogColors.GetAll);

            return new GetAllUsersResult(users);
        }
    }
}
