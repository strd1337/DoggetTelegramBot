using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Privileges
{
    public sealed class GetUserPrivilegesQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetUserPrivilegesQuery, GetUserPrivilegesResult>
    {
        public async Task<ErrorOr<GetUserPrivilegesResult>> Handle(
            GetUserPrivilegesQuery request,
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
                    TelegramEvents.Message);

                return Errors.User.NotFound;
            }
            else
            {
                logger.LogCommon(
                    Constants.User.Messages.Retrieved(request.TelegramId),
                    TelegramEvents.Message);

                return new GetUserPrivilegesResult([.. user.Privileges]);
            }
        }
    }
}
