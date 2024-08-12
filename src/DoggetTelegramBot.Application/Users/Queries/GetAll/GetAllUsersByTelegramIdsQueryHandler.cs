using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;

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
                    UserConstants.Logging.NotFoundRetrieved(request.TelegramIds),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.GetAll);

                return Errors.User.SomeNotFound;
            }

            logger.LogCommon(
                UserConstants.Logging.Retrieved(request.TelegramIds),
                TelegramEvents.Message,
                LoggerConstants.Colors.GetAll);

            return new GetAllUsersResult(users);
        }
    }
}
