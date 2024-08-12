using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Common.Constants.Marriage
{
    public static partial class Constants
    {
        public static partial class Marriage
        {
            public static class Logging
            {
                public static string Retrieved(MarriageId marriageId) =>
                   $"Marriage {marriageId.Value} was retrieved.";

                public static string Retrieved(List<MarriageId> marriageIds) =>
                    $"Marriages {string.Join(",", marriageIds.Select(id => id.Value))} were retrieved.";

                public static string NotFoundRetrieved(UserId userId) =>
                    $"Marriages were not retrieved using user id {userId.Value}.";

                public static string NotFoundRetrieved(List<UserId> userIds) =>
                    $"Marriage was not retrieved using user ids {string.Join(",", userIds.Select(id => id.Value))}";

                public static string UpdatedSuccessfully(MarriageId marriageId) =>
                   $"Marriage {marriageId.Value} was successfully updated.";
            }
        }
    }
}
