using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Families.Queries.GetAll.Information
{
    public sealed class GetAllFamiliesInfoByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetAllFamiliesInfoByUserIdQuery, GetAllFamiliesInfoResult>
    {
        public Task<ErrorOr<GetAllFamiliesInfoResult>> Handle(
            GetAllFamiliesInfoByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            List<Family> families =
            [
                .. unitOfWork.GetRepository<Family, FamilyId>()
                    .GetWhere(f => f.Members.Any(m => m.UserId == request.UserId && !m.IsDeleted) &&
                            !f.IsDeleted,
                        nameof(Family.Members))
,
            ];

            if (families.Count == 0)
            {
                logger.LogCommon(
                    Constants.Family.Messages.NotFoundRetrieved(request.UserId),
                    TelegramEvents.Message,
                    Constants.LogColors.GetAll);

                return Task.FromResult<ErrorOr<GetAllFamiliesInfoResult>>(
                    Errors.Family.NotFound);
            }

            List<FamilyId> familyIds = [.. families.Select(f => f.FamilyId)];

            logger.LogCommon(
                Constants.Family.Messages.Retrieved(familyIds),
                TelegramEvents.Message,
                Constants.LogColors.GetAll);

            return Task.FromResult<ErrorOr<GetAllFamiliesInfoResult>>(
                    new GetAllFamiliesInfoResult([.. families]));
        }
    }
}
