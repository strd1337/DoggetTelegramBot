using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Users.Queries.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Common.Errors;

namespace DoggetTelegramBot.Application.Users.Queries.Get
{
    public sealed class GetUserPrivilegesQueryHandler(IUnitOfWork unitOfWork) :
        IQueryHandler<GetUserPrivilegesQuery, GetUserPrivilegesResult>
    {
        public async Task<ErrorOr<GetUserPrivilegesResult>> Handle(
            GetUserPrivilegesQuery request,
            CancellationToken cancellationToken)
        {
            var user = await unitOfWork
                .GetRepository<User, UserId>()
                .FirstOrDefaultAsync(
                    u => u.TelegramId == request.TelegramId,
                    cancellationToken);

            return user is null ?
                Errors.User.NotFound :
                new GetUserPrivilegesResult([.. user.Privileges]);
        }
    }
}
