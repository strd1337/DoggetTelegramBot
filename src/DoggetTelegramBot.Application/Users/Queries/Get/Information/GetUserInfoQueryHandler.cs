using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Information
{
    public sealed class GetUserInfoQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetUserInfoQuery, GetUserInfoResult>
    {
        public async Task<ErrorOr<GetUserInfoResult>> Handle(
            GetUserInfoQuery request,
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

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramId),
                TelegramEvents.Message);

            return new GetUserInfoResult(
                user.Nickname,
                user.FirstName,
                user.LastName,
                user.RegisteredDate,
                user.MaritalStatus,
                [.. user.Privileges]);
        }
    }
}
