using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Families.Queries.Get.Information
{
    public sealed class GetFamilyInfoByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetFamilyInfoByUserIdQuery, GetFamilyInfoResult>
    {
        public Task<ErrorOr<GetFamilyInfoResult>> Handle(
            GetFamilyInfoByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var family = unitOfWork.GetRepository<Family, FamilyId>()
                .GetWhere(
                    f => f.Members.Any(m => m.UserId == request.UserId) && !f.IsDeleted,
                    nameof(Family.Members))
                .FirstOrDefault();

            if (family is null)
            {
                logger.LogCommon(
                    Constants.Family.Messages.NotFoundRetrieved(request.UserId),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Task.FromResult<ErrorOr<GetFamilyInfoResult>>(
                    Errors.Family.NotFound);
            }

            logger.LogCommon(
                Constants.Family.Messages.Retrieved(FamilyId.Create(family.FamilyId.Value)),
                TelegramEvents.Message,
                Constants.LogColors.Get);

            return Task.FromResult<ErrorOr<GetFamilyInfoResult>>(
                    new GetFamilyInfoResult([.. family.Members]));
        }
    }
}
