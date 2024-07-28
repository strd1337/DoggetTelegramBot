using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Family
        {
            public static class Messages
            {
                public static string CreateRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Create a family request started." :
                    $"Create a family request ended.";

                public static string DeleteRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Delete a family request started." :
                    $"Delete a family request ended.";

                public static string GetInformationRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get family information request started." :
                    $"Get family information request ended.";

                public static string Created(FamilyId familyId) =>
                    $"Family {familyId.Value} was created.";

                public static string Deleted(FamilyId familyId) =>
                    $"Family {familyId.Value} was deleted.";

                public static string Retrieved(FamilyId familyId) =>
                    $"Family {familyId.Value} was retrieved.";

                public static string NotFoundRetrieved(UserId userId) =>
                    $"Family was not retrieved using user id {userId.Value}.";

                public static string NotFoundRetrieved(List<UserId> userIds) =>
                    $"Family was not retrieved using user ids {string.Join(",", userIds.Select(id => id.Value))}.";
            }
        }
    }
}
