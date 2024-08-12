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
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using InventoryConstants = DoggetTelegramBot.Domain.Common.Constants.Inventory.Constants.Inventory;
using MarriageConstants = DoggetTelegramBot.Domain.Common.Constants.Marriage.Constants.Marriage;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

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
                    UserConstants.Logging.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.GroupAction,
                    LoggerConstants.Colors.Get);

                return Errors.User.NotFound;
            }

            logger.LogCommon(
                UserConstants.Logging.Retrieved(request.TelegramId),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Get);

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
                UserConstants.Logging.UpdatedSuccessfully(request.TelegramId),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Update);

            logger.LogCommon(
                InventoryConstants.Logging.UpdatedSuccessfully(user.InventoryId),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Update);

            if (deleteMarriageResult.Value is not null)
            {
                logger.LogCommon(
                    MarriageConstants.Logging.UpdatedSuccessfully(deleteMarriageResult.Value.MarriageId),
                    TelegramEvents.GroupAction,
                    LoggerConstants.Colors.Update);

                logger.LogCommon(
                   FamilyConstants.Logging.UpdatedSuccessfully(deleteMarriageResult.Value.FamilyId),
                   TelegramEvents.GroupAction,
                   LoggerConstants.Colors.Update);
            }
            else if (deleteMemberResult is not null)
            {
                logger.LogCommon(
                   FamilyConstants.Logging.UpdatedSuccessfully(deleteMemberResult.Value.Value.FamilyIds),
                   TelegramEvents.GroupAction,
                   LoggerConstants.Colors.Update);
            }

            return new DeleteUserResult();
        }

        private async Task<ErrorOr<DeleteInventoryResult>> DeleteInventoryByInventoryIdAsync(
            InventoryId inventoryId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                InventoryConstants.Requests.Update(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            DeleteInventoryByInventoryIdCommand command = new(inventoryId);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                InventoryConstants.Requests.Update(false),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            return result;
        }

        private async Task<ErrorOr<DeleteMarriageResult>> DeleteMarriageByUserIdAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                MarriageConstants.Requests.Delete(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            DeleteMarriageCommand command = new(userId);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                MarriageConstants.Requests.Delete(false),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            return result;
        }

        private async Task<ErrorOr<DeleteMemberFromAllFamiliesResult>> DeleteMemberFromAllFamiliesAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                FamilyConstants.Requests.DeleteMemberFromAllFamilies(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            DeleteMemberFromAllFamiliesCommand command = new(userId);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                FamilyConstants.Requests.DeleteMemberFromAllFamilies(false),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            return result;
        }
    }
}
