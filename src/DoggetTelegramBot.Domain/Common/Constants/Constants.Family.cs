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
                public static string GetInformationRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get family information request started." :
                    $"Get family information request ended.";

                public static string Retrieved(FamilyId familyId) =>
                    $"Family {familyId.Value} was retrieved.";

                public static string NotFoundRetrieved(UserId userId) =>
                    $"Family was not retrieved using user id {userId.Value}.";
            }
        }
    }
}
