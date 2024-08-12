using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Common.Constants.Family
{
    public static partial class Constants
    {
        public static partial class Family
        {
            public static class Logging
            {
                public static string Retrieved(FamilyId familyId) =>
                   $"Family {familyId.Value} was retrieved.";

                public static string Retrieved(List<FamilyId> familyIds) =>
                    $"Families {string.Join(",", familyIds.Select(id => id.Value))} were retrieved.";

                public static string NotFoundRetrieved(UserId userId) =>
                    $"Families were not retrieved using user id {userId.Value}.";

                public static string NotFoundRetrieved(List<UserId> userIds) =>
                    $"Family was not retrieved using user ids {string.Join(",", userIds.Select(id => id.Value))}.";

                public static string Created(FamilyId familyId) =>
                    $"Family {familyId.Value} was created.";

                public static string Deleted(FamilyId familyId) =>
                    $"Family {familyId.Value} was deleted.";

                public static string UpdatedSuccessfully(FamilyId familyId) =>
                   $"Family {familyId.Value} was successfully updated.";

                public static string UpdatedSuccessfully(List<FamilyId> familyIds) =>
                   $"Families {string.Join(",", familyIds.Select(id => id.Value))} were successfully updated.";
            }
        }
    }
}
