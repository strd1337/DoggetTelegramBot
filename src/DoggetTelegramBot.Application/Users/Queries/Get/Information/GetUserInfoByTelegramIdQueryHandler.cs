using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Application.Marriages.Queries.Get.Information;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Information
{
    public sealed class GetUserInfoByTelegramIdQueryHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger,
        IMediator mediator) : IQueryHandler<GetUserInfoByTelegramIdQuery, GetUserInfoResult>
    {
        private readonly IBotLogger logger = logger;

        public async Task<ErrorOr<GetUserInfoResult>> Handle(
            GetUserInfoByTelegramIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = unitOfWork.GetRepository<User, UserId>()
                .GetWhere(u => u.TelegramId == request.TelegramId)
                .Select(u => new
                {
                    UserId = UserId.Create(u.Id.Value),
                    u.Username,
                    u.RegisteredDate,
                    u.MaritalStatus,
                    u.Privileges
                })
                .FirstOrDefault();

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Message);

                return Errors.User.NotFound;
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramId),
                TelegramEvents.Message);

            var marriage = await GetUserMarriageInfoAsync(
                user.UserId,
                cancellationToken);

            return new GetUserInfoResult(
                user.Username,
                user.RegisteredDate,
                user.MaritalStatus,
                [.. user.Privileges],
                marriage);
        }

        private async Task<MarriageDto?> GetUserMarriageInfoAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
               Constants.Marriage.Messages.GetInformationRequest(),
               TelegramEvents.Message);

            GetMarriageInfoByUserIdQuery query = new(userId);
            var result = await mediator.Send(query, cancellationToken);

            MarriageDto? marriage = null;

            if (!result.IsError)
            {
                var spouseIds = result.Value.SpouseIds;

                List<SpouseDto> userSpouses =
                [
                    .. unitOfWork.GetRepository<User, UserId>()
                        .GetWhere(u => spouseIds.Contains(u.Id))
                        .Select(u => new SpouseDto(u.Username))
,
                ];

                marriage = new MarriageDto(
                    userSpouses,
                    result.Value.MarriageDate,
                    result.Value.DivorceDate,
                    result.Value.Type,
                    result.Value.Status);
            }

            logger.LogCommon(
                Constants.Marriage.Messages.GetInformationRequest(false),
                TelegramEvents.Message);

            return marriage;
        }
    }
}
