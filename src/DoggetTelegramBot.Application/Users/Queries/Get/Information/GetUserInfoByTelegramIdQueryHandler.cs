using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.DTOs;
using DoggetTelegramBot.Application.Families.Queries.GetAll.Information;
using DoggetTelegramBot.Application.Marriages.Queries.GetAll.Information;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using MarriageConstants = DoggetTelegramBot.Domain.Common.Constants.Marriage.Constants.Marriage;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

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
                    UserId = UserId.Create(u.UserId.Value),
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
                    UserConstants.Logging.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Message,
                    LoggerConstants.Colors.Get);

                return Errors.User.NotFound;
            }

            logger.LogCommon(
                UserConstants.Logging.Retrieved(request.TelegramId),
                TelegramEvents.Message,
                LoggerConstants.Colors.Get);

            var marriages = await GetUserMarriagesInfoAsync(
                user.UserId,
                cancellationToken);

            var families = await GetUserFamiliesInfoAsync(
                user.UserId,
                cancellationToken);

            return new GetUserInfoResult(
                user.Username,
                user.Nickname,
                user.FirstName,
                user.RegisteredDate,
                user.MaritalStatus,
                [.. user.Privileges],
                marriages,
                families);
        }

        private async Task<List<MarriageDto>> GetUserMarriagesInfoAsync(
            UserId userId,
            CancellationToken cancellationToken)
        {
            logger.LogCommon(
                MarriageConstants.Requests.GetInformation(),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            GetAllMarriagesInfoByUserIdQuery query = new(userId);
            var result = await mediator.Send(query, cancellationToken);

            List<MarriageDto> marriages = [];

            if (!result.IsError)
            {
                var allMarriages = result.Value.Marriages;

                marriages = allMarriages.Select(marriage =>
                {
                    List<SpouseDto> spouses =
                    [
                        .. unitOfWork.GetRepository<User, UserId>()
                            .GetWhere(u => marriage.SpouseIds.Contains(u.UserId))
                            .Select(u => new SpouseDto(u.Username, u.Nickname, u.FirstName))
                    ];

                    return new MarriageDto(
                        spouses,
                        marriage.MarriageDate,
                        marriage.DivorceDate,
                        marriage.Type,
                        marriage.Status);

                }).ToList();
            }

            logger.LogCommon(
                MarriageConstants.Requests.GetInformation(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return marriages;
        }


        private async Task<List<FamilyDto>> GetUserFamiliesInfoAsync(
           UserId userId,
           CancellationToken cancellationToken)
        {
            logger.LogCommon(
               FamilyConstants.Requests.GetAllInformation(),
               TelegramEvents.Message,
               LoggerConstants.Colors.Request);

            GetAllFamiliesInfoByUserIdQuery query = new(userId);
            var result = await mediator.Send(query, cancellationToken);

            List<FamilyDto>? families = [];

            if (!result.IsError)
            {
                List<UserId> userIds = result.Value.Families
                    .SelectMany(family => family.Members)
                    .Select(m => UserId.Create(m.UserId.Value))
                    .Distinct()
                    .ToList();

                var users = unitOfWork.GetRepository<User, UserId>()
                    .GetWhere(u => userIds.Contains(u.UserId))
                    .Select(u => new
                    {
                        UserId = u.UserId.Value,
                        u.Username,
                        u.Nickname,
                        u.FirstName,
                    })
                    .ToList();

                var usersDictionary = users.ToDictionary(u => u.UserId);

                foreach (var family in result.Value.Families)
                {
                    List<FamilyMemberDto> members = family.Members
                        .OrderBy(m => m.UserId.Value)
                        .Select(m =>
                        {
                            var userIdValue = m.UserId.Value;
                            var user = usersDictionary[userIdValue];

                            return new FamilyMemberDto(
                                user.Username,
                                user.Nickname,
                                user.FirstName,
                                m.Role);
                        })
                        .ToList();

                    families.Add(new FamilyDto(members));
                }
            }

            logger.LogCommon(
                FamilyConstants.Requests.GetAllInformation(false),
                TelegramEvents.Message,
                LoggerConstants.Colors.Request);

            return families;
        }
    }
}
