using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Marriage
        {
            public const int MaxSpousesCount = 5;

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

                public static string CreateMarriageRequest(bool isStarted = true) =>
                   isStarted ?
                   $"Create new marriage request started." :
                   $"Create new marriage request ended.";

                public static string WrongType(string typeName) =>
                   $"{typeName} type needs to have more spouses.";
            }

            public static class ReplyKeys
            {
                public const string CreateMarriage = $"{BotNickname} +marriage";
            }
        }
    }
}
