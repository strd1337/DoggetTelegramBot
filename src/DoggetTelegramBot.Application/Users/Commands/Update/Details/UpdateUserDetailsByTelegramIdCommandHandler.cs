using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Commands.Update.Details
{
    public sealed class UpdateUserDetailsByTelegramIdCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        ICacheService cacheService) : ICommandHandler<UpdateUserDetailsByTelegramIdCommand, UpdateResult>
    {
        public async Task<ErrorOr<UpdateResult>> Handle(
            UpdateUserDetailsByTelegramIdCommand request,
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

                return UpdateResult.Stop;
            }

            user.UpdateDetails(request.Username, request.FirstName);

            await userRepository.UpdateAsync(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await RemoveKeysFromCacheAsync(user, cancellationToken);

            return UpdateResult.Continue;
        }

        private async Task RemoveKeysFromCacheAsync(
            User user,
            CancellationToken cancellationToken)
        {
            string[] keys =
            [
                CacheKeyGenerator.UserExistsWithTelegramId(user.TelegramId),
                CacheKeyGenerator.GetUserInfoByTelegramId(user.TelegramId),
                CacheKeyGenerator.GetFamilyInfoByUserId(UserId.Create(user.Id.Value)),
                CacheKeyGenerator.GetMarriageInfoByUserId(UserId.Create(user.Id.Value))
            ];

            var removalTasks = keys.Select(key => cacheService.RemoveAsync(key, cancellationToken));
            await Task.WhenAll(removalTasks);
        }
    }
}
