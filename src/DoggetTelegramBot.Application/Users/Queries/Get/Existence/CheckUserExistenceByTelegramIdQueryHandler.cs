using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Users.Commands.Register;
using DoggetTelegramBot.Application.Users.Commands.Update.Details;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using MediatR;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Existence
{
    public sealed class CheckUserExistenceByTelegramIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IBotLogger logger) : IQueryHandler<CheckUserExistenceByTelegramIdQuery, UpdateResult>
    {
        public async Task<ErrorOr<UpdateResult>> Handle(
            CheckUserExistenceByTelegramIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = unitOfWork.GetRepository<User, UserId>()
                .GetWhere(u => u.TelegramId == request.TelegramId)
                .Select(u => new
                {
                    u.TelegramId,
                    u.Username,
                    u.FirstName,
                    u.InventoryId,
                    u.IsDeleted,
                })
                .FirstOrDefault();

            if (user is null)
            {
                logger.LogCommon(
                    Constants.User.Messages.NotFoundRetrieved(request.TelegramId),
                    TelegramEvents.Register,
                    Constants.LogColors.Get);

                RegisterUserCommand command = new(
                    request.TelegramId,
                    request.Username,
                    request.FirstName);

                _ = await mediator.Send(command, cancellationToken);
            }
            else
            {
                logger.LogCommon(
                    Constants.User.Messages.Retrieved(request.TelegramId),
                    TelegramEvents.Register,
                    Constants.LogColors.Get);

                if (user.FirstName != request.FirstName ||
                    user.Username != request.Username ||
                    user.IsDeleted is true)
                {
                    logger.LogCommon(
                       Constants.User.Messages.UpdateRequest(),
                       TelegramEvents.Register,
                       Constants.LogColors.Request);

                    UpdateUserDetailsByTelegramIdCommand command = new(
                        request.TelegramId,
                        request.Username,
                        request.FirstName,
                        user.IsDeleted);

                    var result = await mediator.Send(command, cancellationToken);

                    logger.LogCommon(
                       Constants.User.Messages.UpdateRequest(false),
                       TelegramEvents.Register,
                       Constants.LogColors.Request);

                    return result.Value;
                }
            }

            return UpdateResult.Continue;
        }
    }
}
