using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Marriage
        {
            public static class Messages
            {
                public static string GetInformationRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get marriage information request started." :
                    $"Get marriage information request ended.";

                public static string Retrieved(MarriageId marriageId) =>
                    $"Marriage {marriageId.Value} was retrieved.";

                public static string NotFoundRetrieved(UserId userId) =>
                    $"Marriage was not retrieved using user id {userId.Value}.";
            }
        }
    }
}
