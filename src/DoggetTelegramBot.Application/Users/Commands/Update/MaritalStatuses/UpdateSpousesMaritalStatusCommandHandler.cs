using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;

namespace DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses
{
    public sealed class UpdateSpousesMaritalStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : ICommandHandler<UpdateSpousesMaritalStatusCommand, UpdateSpousesMaritalStatusResult>
    {
        public async Task<ErrorOr<UpdateSpousesMaritalStatusResult>> Handle(
            UpdateSpousesMaritalStatusCommand request,
            CancellationToken cancellationToken)
        {
            List<User> spouses =
            [
                .. unitOfWork.GetRepository<User, UserId>()
                    .GetWhere(u => request.SpouseIds.Contains(u.UserId))
            ];

            List<long> telegramIds = spouses.Select(s => s.TelegramId).ToList();

            logger.LogCommon(
                UserConstants.Logging.Retrieved(telegramIds),
                TelegramEvents.Message,
                LoggerConstants.Colors.Get);

            spouses.ForEach(s => s.UpdateMaritalStatus(request.MaritalStatus));

            await unitOfWork.GetRepository<User, UserId>()
                .UpdateAsync(spouses);

            return new UpdateSpousesMaritalStatusResult();
        }
    }
}
