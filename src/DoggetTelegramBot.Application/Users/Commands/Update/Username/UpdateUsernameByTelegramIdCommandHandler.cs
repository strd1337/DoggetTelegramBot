using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Users.Commands.Update.Username
{
    public sealed class UpdateUsernameByTelegramIdCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : ICommandHandler<UpdateUsernameByTelegramIdCommand, UpdateUsernameResult>
    {
        public async Task<ErrorOr<UpdateUsernameResult>> Handle(
            UpdateUsernameByTelegramIdCommand request,
            CancellationToken cancellationToken)
        {
            var userRepository = unitOfWork.GetRepository<User, UserId>();

            var user = userRepository
                .GetWhere(u => u.TelegramId == request.TelegramId)
                .FirstOrDefault();

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                TelegramEvents.Register);

                return Errors.User.NotFound;
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramId),
                TelegramEvents.Message);

            user.Update(request.Username);

            await userRepository.UpdateAsync(user!);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.UpdatedSuccessfully(request.TelegramId),
                TelegramEvents.Message);

            return new UpdateUsernameResult(request.Username);
        }
    }
}
