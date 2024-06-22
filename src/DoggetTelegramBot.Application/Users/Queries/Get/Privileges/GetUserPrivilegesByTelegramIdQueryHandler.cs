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
    public sealed class GetUserPrivilegesByTelegramIdQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetUserPrivilegesByTelegramIdQuery, GetUserPrivilegesResult>
    {
        public Task<ErrorOr<GetUserPrivilegesResult>> Handle(
            GetUserPrivilegesByTelegramIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = unitOfWork.GetRepository<User, UserId>()
                .GetWhere(u => u.TelegramId == request.TelegramId)
                .Select(u => new
                {
                    u.Privileges
                })
                .FirstOrDefault();

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Task.FromResult<ErrorOr<GetUserPrivilegesResult>>(
                    Errors.User.NotFound);
            }
            else
            {
                logger.LogCommon(
                    Constants.User.Messages.Retrieved(request.TelegramId),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Task.FromResult<ErrorOr<GetUserPrivilegesResult>>(
                    new GetUserPrivilegesResult([.. user.Privileges]));
            }
        }
    }
}
