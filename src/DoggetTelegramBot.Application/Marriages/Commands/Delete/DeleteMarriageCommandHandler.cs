using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Commands.Delete;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using ErrorOr;
using MediatR;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using MarriageConstants = DoggetTelegramBot.Domain.Common.Constants.Marriage.Constants.Marriage;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

namespace DoggetTelegramBot.Application.Marriages.Commands.Delete
{
    public sealed class DeleteMarriageCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        IMediator mediator) : ICommandHandler<DeleteMarriageCommand, DeleteMarriageResult>
    {
        public async Task<ErrorOr<DeleteMarriageResult>> Handle(
            DeleteMarriageCommand request,
            CancellationToken cancellationToken)
        {
            var marriageRepository = unitOfWork.GetRepository<Marriage, MarriageId>();

            var marriage = await marriageRepository
                .FirstOrDefaultAsync(
                    m => m.SpouseIds.Any(id => id.Value == request.UserId.Value) && !m.IsDeleted,
                    cancellationToken);

            if (marriage is null)
            {
                logger.LogCommon(
                    MarriageConstants.Logging.NotFoundRetrieved(request.UserId),
                    TelegramEvents.GroupAction,
                    LoggerConstants.Colors.Get);

                return Errors.Marriage.NotFound;
            }

            logger.LogCommon(
                MarriageConstants.Logging.Retrieved(marriage.MarriageId),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Get);

            List<UserId> spouseIds = [.. marriage.SpouseIds];

            var deleteFamilyResult = await DeleteFamilyBySpouseIdsAsync(
                spouseIds,
                cancellationToken);

            if (deleteFamilyResult.IsError)
            {
                return deleteFamilyResult.Errors;
            }

            await UpdateSpousesMaritalStatus(spouseIds, cancellationToken);

            marriage.Delete();

            await marriageRepository.UpdateAsync(marriage);

            return new DeleteMarriageResult(
                marriage.MarriageId,
                deleteFamilyResult.Value.FamilyId);
        }

        private async Task<ErrorOr<FamilyResult>> DeleteFamilyBySpouseIdsAsync(
           List<UserId> spouseIds,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
                FamilyConstants.Requests.Delete(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            DeleteFamilyBySpouseIdsCommand command = new(spouseIds);

            var result = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                FamilyConstants.Requests.Delete(false),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Request);

            return result;
        }

        private async Task UpdateSpousesMaritalStatus(
            List<UserId> spouseIds,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                UserConstants.Requests.UpdateMaritalStatus(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Update);

            UpdateSpousesMaritalStatusCommand command = new(
                spouseIds, MaritalStatus.NotMarried);

            _ = await mediator.Send(command, cancellationToken);

            logger.LogCommon(
                UserConstants.Requests.UpdateMaritalStatus(),
                TelegramEvents.GroupAction,
                LoggerConstants.Colors.Update);
        }
    }
}
