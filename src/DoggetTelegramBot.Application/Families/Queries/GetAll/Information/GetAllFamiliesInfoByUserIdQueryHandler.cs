using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

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
                    FamilyConstants.Logging.NotFoundRetrieved(request.UserId),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.GetAll);

                return Task.FromResult<ErrorOr<GetAllFamiliesInfoResult>>(
                    Errors.Family.NotFound);
            }

            List<FamilyId> familyIds = [.. families.Select(f => f.FamilyId)];

            logger.LogCommon(
                FamilyConstants.Logging.Retrieved(familyIds),
                TelegramEvents.Message,
                LoggerConstants.Colors.GetAll);

            return Task.FromResult<ErrorOr<GetAllFamiliesInfoResult>>(
                    new GetAllFamiliesInfoResult([.. families]));
        }
    }
}
