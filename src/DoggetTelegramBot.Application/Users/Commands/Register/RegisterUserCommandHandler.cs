using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Inventories.Commands.Create;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using ErrorOr;
using MediatR;
using PRTelegramBot.Models.Enums;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using UserConstants = DoggetTelegramBot.Domain.Common.Constants.User.Constants.User;
using InventoryConstants = DoggetTelegramBot.Domain.Common.Constants.Inventory.Constants.Inventory;

namespace DoggetTelegramBot.Application.Users.Commands.Register
{
    public sealed class RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IMediator mediator,
        IBotLogger logger) : ICommandHandler<RegisterUserCommand, UpdateResult>
    {
        public async Task<ErrorOr<UpdateResult>> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            CreateInventoryCommand command = new();
            var result = await mediator.Send(command, cancellationToken);

            User user = User.Create(
                request.TelegramId,
                request.Username,
                request.FirstName,
                dateTimeProvider.UtcNow,
                result.Value.InventoryId,
                MaritalStatus.NotMarried);

            var userRepository = unitOfWork.GetRepository<User, UserId>();

            await userRepository.AddAsync(user, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogCommon(
                UserConstants.Logging.Registered(user.TelegramId),
                TelegramEvents.Register,
                LoggerConstants.Colors.Register);

            logger.LogCommon(
                InventoryConstants.Logging.Created(user.InventoryId.Value),
                TelegramEvents.Register,
                LoggerConstants.Colors.Register);

            return UpdateResult.Continue;
        }
    }
}
