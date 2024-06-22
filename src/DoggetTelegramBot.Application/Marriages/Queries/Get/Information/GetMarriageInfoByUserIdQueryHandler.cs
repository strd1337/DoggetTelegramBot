using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Marriages.Queries.Get.Information
{
    public sealed class GetMarriageInfoByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetMarriageInfoByUserIdQuery, GetMarriageInfoResult>
    {
        public async Task<ErrorOr<GetMarriageInfoResult>> Handle(
            GetMarriageInfoByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var marriage = await unitOfWork.GetRepository<Marriage, MarriageId>()
                .FirstOrDefaultAsync(
                    m => m.SpouseIds.Any(id => id.Value == request.UserId.Value),
                    cancellationToken);

            if (marriage is null)
            {
                logger.LogCommon(
                    Constants.Marriage.Messages.NotFoundRetrieved(request.UserId),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Errors.Marriage.NotFound;
            }

            List<UserId> userSpouseIds = marriage.SpouseIds
                .Where(id => id != request.UserId)
                .ToList();

            logger.LogCommon(
                Constants.Marriage.Messages.Retrieved(MarriageId.Create(marriage.Id.Value)),
                TelegramEvents.Message,
                Constants.LogColors.Get);

            return new GetMarriageInfoResult(
                userSpouseIds,
                marriage.MarriageDate,
                marriage.DivorceDate,
                marriage.Type,
                marriage.Status);
        }
    }
}
