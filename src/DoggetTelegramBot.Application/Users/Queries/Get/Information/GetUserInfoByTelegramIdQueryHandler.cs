using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.DTOs;
using DoggetTelegramBot.Application.Families.Queries.Get.Information;
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
                .GetWhere(u => u.TelegramId == request.TelegramId && !u.IsDeleted)
                .Select(u => new
                {
                    UserId = UserId.Create(u.Id.Value),
                    u.Username,
                    u.Nickname,
                    u.FirstName,
                    u.RegisteredDate,
                    u.MaritalStatus,
                    u.Privileges
                })
                .FirstOrDefault();

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Message,
                    Constants.LogColors.Get);

                return Errors.User.NotFound;
            }

            logger.LogCommon(
                Constants.User.Messages.Retrieved(request.TelegramId),
                TelegramEvents.Message,
                Constants.LogColors.Get);

            var marriage = await GetUserMarriageInfoAsync(
                user.UserId,
                cancellationToken);

            var family = await GetUserFamilyInfoAsync(
                user.UserId,
                cancellationToken);

            return new GetUserInfoResult(
                user.Username,
                user.Nickname,
                user.FirstName,
                user.RegisteredDate,
                user.MaritalStatus,
                [.. user.Privileges],
                marriage,
                family);
        }

        private async Task<MarriageDto?> GetUserMarriageInfoAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
               Constants.Marriage.Messages.GetInformationRequest(),
               TelegramEvents.Message,
               Constants.LogColors.Request);

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
                        .Select(u => new SpouseDto(u.Username, u.Nickname, u.FirstName))
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
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return marriage;
        }

        private async Task<FamilyDto?> GetUserFamilyInfoAsync(
           UserId userId,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
               Constants.Family.Messages.GetInformationRequest(),
               TelegramEvents.Message,
               Constants.LogColors.Request);

            GetFamilyInfoByUserIdQuery query = new(userId);
            var result = await mediator.Send(query, cancellationToken);

            FamilyDto? family = null;

            if (!result.IsError)
            {
                List<UserId> userIds = result.Value.Members
                    .Select(m => UserId.Create(m.UserId.Value))
                    .ToList();

                var users = unitOfWork.GetRepository<User, UserId>()
                    .GetWhere(u => userIds.Contains(u.Id))
                    .Select(u => new
                    {
                        UserId = u.Id.Value,
                        u.Username,
                        u.Nickname,
                        u.FirstName,
                    })
                    .OrderBy(u => u.UserId)
                    .ToList();

                var membersResult = result.Value.Members.OrderBy(m => m.UserId.Value);

                List<FamilyMemberDto> members = membersResult
                    .Join(
                        users,
                        member => member.UserId.Value,
                        user => user.UserId,
                        (member, user) => new FamilyMemberDto(
                            user.Username,
                            user.Nickname,
                            user.FirstName,
                            member.Role
                        )
                    )
                    .ToList();

                family = new(members);
            }

            logger.LogCommon(
                Constants.Family.Messages.GetInformationRequest(false),
                TelegramEvents.Message,
                Constants.LogColors.Request);

            return family;
        }
    }
}
