using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using ErrorOr;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll.Spouses
{
    public sealed class GetSpousesByTelegramIdsQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetSpousesByTelegramIdsQuery, GetSpousesResult>
    {
        public async Task<ErrorOr<GetSpousesResult>> Handle(
            GetSpousesByTelegramIdsQuery request,
            CancellationToken cancellationToken)
        {
            List<User> users =
            [
                .. await unitOfWork.GetRepository<User, UserId>()
                    .GetWhereAsync(
                        u => request.TelegramIds.Contains(u.TelegramId) && !u.IsDeleted,
                        cancellationToken)
            ];

            var eligibleSpouses = FilterSpousesByMaritalStatus(users, request.IsGetMarried);

            if (eligibleSpouses.Count != request.TelegramIds.Count)
            {
                string errorDescription = request.IsGetMarried ?
                    Errors.User.SomeMarried.Description :
                    Errors.User.SomeNotMarriedOrDivorced.Description;

                return LogAndReturnError(request.TelegramIds, errorDescription, request.IsGetMarried);
            }

            logger.LogCommon(
                UserConstants.Logging.Retrieved(request.TelegramIds),
                TelegramEvents.Message,
                LoggerConstants.Colors.GetAll);

            return new GetSpousesResult(eligibleSpouses);
        }

        private static List<User> FilterSpousesByMaritalStatus(List<User> users, bool isGetMarried) =>
            users.Where(u =>
                    isGetMarried ?
                    u.MaritalStatus is MaritalStatus.NotMarried or MaritalStatus.Divorced :
                    u.MaritalStatus is MaritalStatus.Married)
                .ToList();

        private Error LogAndReturnError(
            List<long> spouseIds,
            string errorDescription,
            bool isGetMarried)
        {
            string errorMessage = UserConstants.Logging.NotFoundRetrieved(
                spouseIds, errorDescription);

            var eventType = TelegramEvents.Message;
            var logColor = LoggerConstants.Colors.GetAll;

            logger.LogCommon(errorMessage, eventType, logColor);

            return isGetMarried ?
                Errors.User.SomeMarried :
                Errors.User.SomeNotMarriedOrDivorced;
        }
    }
}
