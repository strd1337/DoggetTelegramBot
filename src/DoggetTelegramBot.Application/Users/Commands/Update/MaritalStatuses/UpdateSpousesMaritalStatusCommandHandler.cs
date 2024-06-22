using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;

namespace DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses
{
    public sealed class UpdateSpousesMaritalStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) :
            ICommandHandler<UpdateSpousesMaritalStatusCommand, UpdateSpousesMaritalStatusResult>
    {
        public async Task<ErrorOr<UpdateSpousesMaritalStatusResult>> Handle(
            UpdateSpousesMaritalStatusCommand request,
            CancellationToken cancellationToken)
        {
            var spouses = request.Spouses;

            spouses.ForEach(s => s.UpdateMaritalStatus(request.MaritalStatus));

            await unitOfWork.GetRepository<User, UserId>()
                .UpdateAsync(spouses);

            List<long> telegramIds = spouses.Select(s => s.TelegramId).ToList();

            logger.LogCommon(
               Constants.User.Messages.UpdatedSuccessfully(telegramIds),
               TelegramEvents.Message);

            return new UpdateSpousesMaritalStatusResult();
        }
    }
}
