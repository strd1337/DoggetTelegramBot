using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Application.Users.Queries.Get;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;

namespace DoggetTelegramBot.Application.Families.Commands.Add
{
    public sealed class AddToFamilyCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        IMediator mediator,
        ITransactionService transactionService) : ICommandHandler<AddToFamilyCommand, AddToFamilyResult>
    {
        public async Task<ErrorOr<AddToFamilyResult>> Handle(
            AddToFamilyCommand request,
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

            var newFamilyMemberResult = await CheckAndGetNewFamilyMember(
                request.NewMemberTelegramId,
                family,
                cancellationToken);

            if (newFamilyMemberResult.IsError)
            {
                return newFamilyMemberResult.Errors;
            }

            var newFamilyUser = newFamilyMemberResult.Value;

            List<UserId> parentIds = family.Members
                .Where(m => m.Role == FamilyRole.Parent)
                .Select(m => m.UserId)
                .ToList();

            decimal amount = GetServiceFeeAmount(request.FamilyRole);

            var transactionResult = await ExecuteServiceFeeAsync(
                parentIds,
                amount,
                cancellationToken);

            if (transactionResult.IsError)
            {
                return transactionResult.Errors;
            }

            FamilyMember newFamilyMember = FamilyMember.Create(
                newFamilyUser.UserId,
                request.FamilyRole);

            family.AddMember(newFamilyMember);

            await familyRepository.UpdateAsync(family);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddToFamilyResult(family.FamilyId);
        }

        private async Task<ErrorOr<User>> CheckAndGetNewFamilyMember(
            long newFamilyMemberTelegramId,
            Family family,
            CancellationToken cancellationToken)
        {
            var newFamilyMemberResult = await GetUserByTelegramId(
                newFamilyMemberTelegramId,
                cancellationToken);

            if (newFamilyMemberResult.IsError)
            {
                return newFamilyMemberResult.Errors;
            }

            var newFamilyUser = newFamilyMemberResult.Value.User;

            bool isParent = family.Members
                .Any(m => m.UserId == newFamilyUser.UserId &&
                    m.Role == FamilyRole.Parent &&
                    !m.IsDeleted);

            if (isParent)
            {
                return Errors.Family.TurnParentIntoChildOrPet;
            }

            bool memberHasFamily = family.Members
                .Any(m => m.UserId == newFamilyUser.UserId &&
                    !(m.Role == FamilyRole.Cat ||
                        m.Role == FamilyRole.Dog ||
                        m.Role == FamilyRole.Son ||
                        m.Role == FamilyRole.Daughter) &&
                    !m.IsDeleted);

            return memberHasFamily ? Errors.Family.NewMemberHasFamily : newFamilyUser;
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

        private async Task<ErrorOr<GetUserResult>> GetUserByTelegramId(
            long telegramId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.User.Messages.GetRequest(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            GetUserByTelegramIdQuery query = new(telegramId);
            var result = await mediator.Send(query, cancellationToken);

            logger.LogCommon(
                Constants.User.Messages.GetRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return result;
        }

        private async Task<ErrorOr<bool>> ExecuteServiceFeeAsync(
           List<UserId> spouseIds,
           decimal amount,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
                Constants.Transaction.Messages.ExecuteServiceFee(),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            var transactionResult = await transactionService.ExecuteServiceFeeAsync(
                spouseIds,
                amount,
                cancellationToken);

            logger.LogCommon(
                Constants.Transaction.Messages.ExecuteServiceFee(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return transactionResult;
        }

        private static decimal GetServiceFeeAmount(FamilyRole familyRole) =>
            familyRole is FamilyRole.Cat or FamilyRole.Dog ?
            Constants.Family.Costs.AddPet :
            Constants.Family.Costs.AddChild;
    }
}
