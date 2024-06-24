using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Users.Commands.Update.Nickname
{
    public sealed class UpdateNicknameByTelegramIdCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        ICacheService cacheService) : ICommandHandler<UpdateNicknameByTelegramIdCommand, UpdateNicknameResult>
    {
        public async Task<ErrorOr<UpdateNicknameResult>> Handle(
            UpdateNicknameByTelegramIdCommand request,
            CancellationToken cancellationToken)
        {
            var userRepository = unitOfWork.GetRepository<User, UserId>();

            var user = await userRepository
                .FirstOrDefaultAsync(
                    u => u.TelegramId == request.TelegramId && !u.IsDeleted,
                    cancellationToken);

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Register,
                    Constants.LogColors.Get);

                return Errors.User.NotFound;
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramId),
                TelegramEvents.Message,
                Constants.LogColors.Get);

            user.UpdateNickname(request.Nickname);

            await userRepository.UpdateAsync(user!);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.UpdatedSuccessfully(request.TelegramId),
                TelegramEvents.Message,
                Constants.LogColors.Update);

            await RemoveKeyFromCacheAsync(user.TelegramId, cancellationToken);

            return new UpdateNicknameResult(user.Nickname);
        }

        private async Task RemoveKeyFromCacheAsync(
            long telegramId,
            CancellationToken cancellationToken)
        {
            string key = CacheKeyGenerator.GetUserInfoByTelegramId(telegramId);
            await cacheService.RemoveAsync(key, cancellationToken);
        }
    }
}
