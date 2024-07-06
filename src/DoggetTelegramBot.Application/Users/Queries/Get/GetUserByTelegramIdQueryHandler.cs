using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Common.Errors;

namespace DoggetTelegramBot.Application.Users.Queries.Get
{
    public sealed class GetUserByTelegramIdQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetUserByTelegramIdQuery, GetUserResult>
    {
        public async Task<ErrorOr<GetUserResult>> Handle(
            GetUserByTelegramIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await unitOfWork.GetRepository<User, UserId>()
                .FirstOrDefaultAsync(
                    u => u.TelegramId == request.TelegramId,
                    cancellationToken);

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Errors.User.NotFound;
            }

            return new GetUserResult(user);
        }
    }
}
