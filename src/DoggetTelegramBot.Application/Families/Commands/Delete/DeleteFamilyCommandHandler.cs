using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Families.Commands.Delete
{
    public sealed class DeleteFamilyCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : ICommandHandler<DeleteFamilyCommand, FamilyResult>
    {
        public async Task<ErrorOr<FamilyResult>> Handle(
            DeleteFamilyCommand request,
            CancellationToken cancellationToken)
        {
            var familyRepository = unitOfWork.GetRepository<Family, FamilyId>();

            var family = unitOfWork.GetRepository<Family, FamilyId>()
                .GetWhere(f =>
                    f.Members.All(m => request.SpouseIds.Contains(m.UserId) && !m.IsDeleted) && !f.IsDeleted,
                    nameof(Family.Members))
                .FirstOrDefault();

            if (family is null)
            {
                logger.LogCommon(
                   Constants.Family.Messages.NotFoundRetrieved(request.SpouseIds),
                   TelegramEvents.Message,
                   Constants.LogColors.Get);

                return Errors.Family.NotFound;
            }

            family.Delete();

            await familyRepository.UpdateAsync(family);

            return new FamilyResult(family.FamilyId);
        }
    }
}
