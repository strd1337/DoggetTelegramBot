using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

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
                        u => request.TelegramIds.Contains(u.TelegramId) &&
                            u.MaritalStatus == MaritalStatus.NotMarried,
                        cancellationToken)
            ];

            List<User> spouses = users
                .Where(u => u.MaritalStatus == MaritalStatus.NotMarried)
                .ToList();

            if (spouses.Count != request.TelegramIds.Count)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrievedWithCause(
                        request.TelegramIds,
                        Errors.User.SomeNotFoundOrMarried.Description),
                    TelegramEvents.Message);

                return Errors.User.SomeNotFoundOrMarried;
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramIds),
                TelegramEvents.Message);

            return new GetSpousesResult(spouses);
        }
    }
}
