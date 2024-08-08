using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Commands.DeleteMember;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Application.Inventories.Commands.Delete;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Application.Marriages.Commands.Delete;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;

namespace DoggetTelegramBot.Application.Users.Commands.Delete
{
    public sealed class DeleteUserByTelegramIdCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        IMediator mediator) : ICommandHandler<DeleteUserByTelegramIdCommand, DeleteUserResult>
    {
        public async Task<ErrorOr<DeleteUserResult>> Handle(
            DeleteUserByTelegramIdCommand request,
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
                    TelegramEvents.GroupAction,
                    Constants.LogColors.Get);

                return Errors.User.NotFound;
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramId),
                TelegramEvents.GroupAction,
                Constants.LogColors.Get);

            var deleteInventoryResult = await DeleteInventoryByInventoryIdAsync(
                user.InventoryId,
                cancellationToken);

            if (deleteInventoryResult.IsError)
            {
                return deleteInventoryResult.Errors;
            }

            var deleteMarriageResult = await DeleteMarriageByUserIdAsync(
                user.UserId,
                cancellationToken);

            ErrorOr<DeleteMemberFromAllFamiliesResult>? deleteMemberResult = null;
            if (deleteMarriageResult.Value is null)
            {
                deleteMemberResult = await DeleteMemberFromAllFamiliesAsync(
                    user.UserId,
                    cancellationToken);

                if (deleteMemberResult.Value.IsError)
                {
                    return deleteMemberResult.Value.Errors;
                }
            }

            user.ClearPrivileges();
            user.Delete();

            await userRepository.UpdateAsync(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.UpdatedSuccessfully(request.TelegramId),
                TelegramEvents.GroupAction,
                Constants.LogColors.Update);

            logger.LogCommon(
                Constants.Inventory.Messages.UpdatedSuccessfully(user.InventoryId),
                TelegramEvents.GroupAction,
                Constants.LogColors.Update);

            if (deleteMarriageResult.Value is not null)
            {
                logger.LogCommon(
                    Constants.Marriage.Messages.UpdatedSuccessfully(deleteMarriageResult.Value.MarriageId),
                    TelegramEvents.GroupAction,
                    Constants.LogColors.Update);

                logger.LogCommon(
                   Constants.Family.Messages.UpdatedSuccessfully(deleteMarriageResult.Value.FamilyId),
                   TelegramEvents.GroupAction,
                   Constants.LogColors.Update);
            }
            else if (deleteMemberResult is not null)
            {
                logger.LogCommon(
                   Constants.Family.Messages.UpdatedSuccessfully(deleteMemberResult.Value.Value.FamilyIds),
                   TelegramEvents.GroupAction,
                   Constants.LogColors.Update);
            }

            return new DeleteUserResult();
        }

        private async Task<ErrorOr<DeleteInventoryResult>> DeleteInventoryByInventoryIdAsync(
            InventoryId inventoryId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Inventory.Messages.UpdateRequest(),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            DeleteInventoryByInventoryIdCommand command = new(inventoryId);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                Constants.Inventory.Messages.UpdateRequest(false),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            return result;
        }

        private async Task<ErrorOr<DeleteMarriageResult>> DeleteMarriageByUserIdAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Marriage.Messages.DeleteRequest(),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            DeleteMarriageCommand command = new(userId);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                Constants.Marriage.Messages.DeleteRequest(false),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            return result;
        }

        private async Task<ErrorOr<DeleteMemberFromAllFamiliesResult>> DeleteMemberFromAllFamiliesAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Family.Messages.DeleteMemberFromAllFamiliesRequest(),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            DeleteMemberFromAllFamiliesCommand command = new(userId);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                Constants.Family.Messages.DeleteMemberFromAllFamiliesRequest(false),
                TelegramEvents.GroupAction,
                Constants.LogColors.Request);

            return result;
        }
    }
}
