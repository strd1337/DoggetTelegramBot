using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Commands.Register;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Existence
{
    public sealed class CheckUserExistenceQueryHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IBotLogger logger) : IQueryHandler<CheckUserExistenceQuery, ResultUpdate>
    {
        public async Task<ErrorOr<ResultUpdate>> Handle(
            CheckUserExistenceQuery request,
            CancellationToken cancellationToken)
        {
            var user = await unitOfWork.GetRepository<User, UserId>()
                .FirstOrDefaultAsync(
                    u => u.TelegramId == request.TelegramId,
                    cancellationToken);

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Register);

                RegisterUserCommand command = new(
                    request.TelegramId,
                    request.Username);

                var result = await mediator.Send(command, cancellationToken);
            }
            else
            {
                logger.LogCommon(
                    Constants.User.Messages.Retrieved(request.TelegramId),
                    TelegramEvents.Register);
            }

            return ResultUpdate.Continue;
        }
    }
}
