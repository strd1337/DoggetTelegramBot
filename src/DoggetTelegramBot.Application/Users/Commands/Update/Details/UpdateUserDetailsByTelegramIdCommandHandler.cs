using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Inventories.Commands.Update;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Commands.Update.Details
{
    public sealed class UpdateUserDetailsByTelegramIdCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        IMediator mediator,
        ICacheService cacheService) : ICommandHandler<UpdateUserDetailsByTelegramIdCommand, UpdateResult>
    {
        public async Task<ErrorOr<UpdateResult>> Handle(
            UpdateUserDetailsByTelegramIdCommand request,
            CancellationToken cancellationToken)
        {
            var userRepository = unitOfWork.GetRepository<User, UserId>();

            var user = await userRepository
                .FirstOrDefaultAsync(
                    u => u.TelegramId == request.TelegramId,
                    cancellationToken);

            if (user is null ||
                (user.IsDeleted is true && request.IsDeleted is false))
            {
                return UpdateResult.Stop;
            }

            if (request.IsDeleted is true)
            {
                var inventoryResult = await UpdateInventoryByInventoryIdAsync(
                    user.InventoryId,
                    0,
                    request.IsDeleted,
                    cancellationToken);

                if (inventoryResult.IsError)
                {
                    logger.LogCommon(
                        Constants.Inventory.Messages.UpdateRequest(false),
                        TelegramEvents.Register,
                        Constants.LogColors.Request);

                    return UpdateResult.Stop;
                }

                user.Restore();
            }

            user.UpdateDetails(request.Username, request.FirstName);

            await userRepository.UpdateAsync(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
                Constants.Inventory.Messages.UpdatedSuccessfully(user.InventoryId),
                TelegramEvents.Register,
                Constants.LogColors.Update);

            logger.LogCommon(
                Constants.Inventory.Messages.UpdateRequest(false),
                TelegramEvents.Register,
                Constants.LogColors.Request);

            logger.LogCommon(
                Constants.User.Messages.UpdatedSuccessfully(request.TelegramId),
                TelegramEvents.Register,
                Constants.LogColors.Update);

            await RemoveKeysFromCacheAsync(user, cancellationToken);

            return UpdateResult.Continue;
        }

        public async Task<ErrorOr<UpdateInventoryResult>> UpdateInventoryByInventoryIdAsync(
            InventoryId inventoryId,
            decimal amount,
            bool isDeleted,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Inventory.Messages.UpdateRequest(),
                TelegramEvents.Register,
                Constants.LogColors.Request);

            UpdateInventoryByInventoryIdCommand command = new(inventoryId, amount, isDeleted);

            var result = await mediator.Send(command, cancellationToken);

            return result;
        }

        private async Task RemoveKeysFromCacheAsync(
            User user,
            CancellationToken cancellationToken)
        {
            string[] keys =
            [
                CacheKeyGenerator.UserExistsWithTelegramId(user.TelegramId),
                CacheKeyGenerator.GetUserInfoByTelegramId(user.TelegramId),
                CacheKeyGenerator.GetFamilyInfoByUserId(UserId.Create(user.UserId.Value)),
                CacheKeyGenerator.GetAllMarriagesInfoByUserId(UserId.Create(user.UserId.Value))
            ];

            var removalTasks = keys.Select(key => cacheService.RemoveAsync(key, cancellationToken));
            await Task.WhenAll(removalTasks);
        }
    }
}
