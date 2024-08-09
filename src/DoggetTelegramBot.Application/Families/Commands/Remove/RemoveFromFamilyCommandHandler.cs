using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.Get;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;
using MediatR;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using TransactionConstants = DoggetTelegramBot.Domain.Common.Constants.Transaction.Constants.Transaction;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

namespace DoggetTelegramBot.Application.Families.Commands.Remove
{
    public sealed class RemoveFromFamilyCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IBotLogger logger,
        ITransactionService transactionService) : ICommandHandler<RemoveFromFamilyCommand, RemoveFromFamilyResult>
    {
        public async Task<ErrorOr<RemoveFromFamilyResult>> Handle(
            RemoveFromFamilyCommand request,
            CancellationToken cancellationToken)
        {
            var familyRepository = unitOfWork.GetRepository<Family, FamilyId>();

            var familyResult = await GetFamilyByParentTelegramIdAsync(
                familyRepository,
                request.ParentTelegramId,
                cancellationToken);

            if (familyResult.IsError)
            {
                return familyResult.Errors;
            }

            var family = familyResult.Value;

            var memberToRemoveResult = await CheckAndGetMemberToRemove(
                request.MemberToRemoveTelegramId,
                family,
                cancellationToken);

            if (memberToRemoveResult.IsError)
            {
                return memberToRemoveResult.Errors;
            }

            var memberToRemove = memberToRemoveResult.Value;

            List<UserId> parentIds = family.Members
                .Where(m => m.Role == FamilyRole.Parent)
                .Select(m => m.UserId)
                .ToList();

            decimal amount = GetServiceFeeAmount(memberToRemove.Role);

            var transactionResult = await ExecuteServiceFeeAsync(
                parentIds,
                amount,
                cancellationToken);

            if (transactionResult.IsError)
            {
                return transactionResult.Errors;
            }

            memberToRemove.Delete();

            await familyRepository.UpdateAsync(family);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RemoveFromFamilyResult(family.FamilyId);
        }

        private async Task<ErrorOr<Family>> GetFamilyByParentTelegramIdAsync(
            IGenericRepository<Family, FamilyId> familyRepository,
            long familyMemberTelegramId,
            CancellationToken cancellationToken)
        {
            var familyMemberResult = await GetUserByTelegramId(
                familyMemberTelegramId,
                cancellationToken);

            if (familyMemberResult.IsError)
            {
                return familyMemberResult.Errors;
            }

            var familyUser = familyMemberResult.Value.User;

            var family = familyRepository
                .GetAll(nameof(Family.Members))
                .FirstOrDefault(f =>
                    f.Members.Any(m => m.UserId == familyUser.UserId &&
                        m.Role == FamilyRole.Parent &&
                        !m.IsDeleted) &&
                    !f.IsDeleted);

            return family is null ? Errors.Family.UserFamilyNotFound : family;
        }

        private async Task<ErrorOr<FamilyMember>> CheckAndGetMemberToRemove(
            long newFamilyMemberTelegramId,
            Family family,
            CancellationToken cancellationToken)
        {
            var memberToRemoveResult = await GetUserByTelegramId(
                newFamilyMemberTelegramId,
                cancellationToken);

            if (memberToRemoveResult.IsError)
            {
                return memberToRemoveResult.Errors;
            }

            var userToRemove = memberToRemoveResult.Value.User;

            bool isParent = family.Members
                .Any(m => m.UserId == userToRemove.UserId &&
                    m.Role == FamilyRole.Parent &&
                    !m.IsDeleted);

            if (isParent)
            {
                return Errors.Family.AttemptToRemoveParent;
            }

            var familyMember = family.Members
                .FirstOrDefault(m => m.UserId == userToRemove.UserId && !m.IsDeleted);

            return familyMember is null ? Errors.Family.FamilyMemberNotFound : familyMember;
        }

        private async Task<ErrorOr<GetUserResult>> GetUserByTelegramId(
            long telegramId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                UserConstants.Requests.Get(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetUserByTelegramIdQuery query = new(telegramId);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                UserConstants.Requests.Get(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return result;
        }

        private async Task<ErrorOr<bool>> ExecuteServiceFeeAsync(
           List<UserId> spouseIds,
           decimal amount,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
                TransactionConstants.Requests.ExecuteServiceFee(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            var transactionResult = await transactionService.ExecuteServiceFeeAsync(
                spouseIds,
                amount,
                cancellationToken);

            logger.LogCommon(
                TransactionConstants.Requests.ExecuteServiceFee(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return transactionResult;
        }

        private static decimal GetServiceFeeAmount(FamilyRole familyRole) =>
            familyRole is FamilyRole.Cat or FamilyRole.Dog ?
            FamilyConstants.Costs.RemovePet :
            FamilyConstants.Costs.RemoveChild;
    }
}
