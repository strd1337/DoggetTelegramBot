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
    public sealed class GetMarriageInfoQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : IQueryHandler<GetMarriageInfoQuery, GetMarriageInfoResult>
    {
        public Task<ErrorOr<GetMarriageInfoResult>> Handle(
            GetMarriageInfoQuery request,
            CancellationToken cancellationToken)
        {
            var marriage = unitOfWork.GetRepository<Marriage, MarriageId>()
                .GetWhere(m => m.SpouseIds.Any(id => id.Value == request.UserId.Value))
                .FirstOrDefault();

            if (marriage is null)
            {
                logger.LogCommon(
                    Constants.Marriage.Messages.NotFoundRetrieved(request.UserId),
                    TelegramEvents.Message);

                return Task.FromResult<ErrorOr<GetMarriageInfoResult>>(
                    Errors.Marriage.NotFound);
            }

            List<UserId> userSpouseIds = marriage.SpouseIds
                .Where(id => id != request.UserId)
                .ToList();

            logger.LogCommon(
                    Constants.Marriage.Messages.Retrieved(MarriageId.Create(marriage.Id.Value)),
                    TelegramEvents.Message);

            return Task.FromResult<ErrorOr<GetMarriageInfoResult>>(new GetMarriageInfoResult(
                userSpouseIds,
                marriage.MarriageDate,
                marriage.DivorceDate,
                marriage.Type,
                marriage.Status));
        }
    }
}
