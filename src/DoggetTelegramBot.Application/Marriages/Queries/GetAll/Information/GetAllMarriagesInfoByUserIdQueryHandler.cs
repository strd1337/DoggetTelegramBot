using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;

namespace DoggetTelegramBot.Application.Marriages.Queries.GetAll.Information
{
    public sealed class GetAllMarriagesInfoByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetAllMarriagesInfoByUserIdQuery, GetAllMarriagesInfoResult>
    {
        public Task<ErrorOr<GetAllMarriagesInfoResult>> Handle(
            GetAllMarriagesInfoByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            List<Marriage> marriages =
            [
                .. unitOfWork.GetRepository<Marriage, MarriageId>()
                    .GetWhere(m => m.SpouseIds.Any(id => id.Value == request.UserId.Value) &&
                        !m.IsDeleted)
                    .OrderBy(m => m.Status == MarriageStatus.Active)
            ];

            if (marriages.Count == 0)
            {
                logger.LogCommon(
                    Constants.Marriage.Messages.NotFoundRetrieved(request.UserId),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Task.FromResult<ErrorOr<GetAllMarriagesInfoResult>>(
                    Errors.Marriage.NotFound);
            }

            List<MarriageId> marriageIds = marriages
                .Select(m => MarriageId.Create(m.MarriageId.Value))
                .ToList();

            logger.LogCommon(
                Constants.Marriage.Messages.Retrieved(marriageIds),
                TelegramEvents.Message,
                Constants.LogColors.Get);

            return Task.FromResult<ErrorOr<GetAllMarriagesInfoResult>>(
                new GetAllMarriagesInfoResult(marriages));
        }
    }
}
