using System.Globalization;
using System.Text;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Family
        {
            public const int AddToFamilyTimeoutInSeconds = 60;

            public static class Messages
            {
                public static string AddToFamilyRequest(bool isStarted = true) =>
                   isStarted ?
                   $"Add to family request started." :
                   $"Add to family request ended.";

                public static string RemoveFromFamilyRequest(bool isStarted = true) =>
                   isStarted ?
                   $"Remove from family request started." :
                   $"Remove from family request ended.";

                public static string CreateRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Create a family request started." :
                    $"Create a family request ended.";

                public static string DeleteRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Delete a family request started." :
                    $"Delete a family request ended.";

                public static string DeleteMemberFromAllFamiliesRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Delete a member from all families request started." :
                    $"Delete a member from all families request ended.";

                public static string GetAllInformationRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get all families information request started." :
                    $"Get all families information request ended.";

                public static string Created(FamilyId familyId) =>
                    $"Family {familyId.Value} was created.";

                public static string Deleted(FamilyId familyId) =>
                    $"Family {familyId.Value} was deleted.";

                public static string Retrieved(FamilyId familyId) =>
                   $"Family {familyId.Value} was retrieved.";

                public static string Retrieved(List<FamilyId> familyIds) =>
                    $"Families {string.Join(",", familyIds.Select(id => id.Value))} were retrieved.";

                public static string NotFoundRetrieved(UserId userId) =>
                    $"Families were not retrieved using user id {userId.Value}.";

                public static string NotFoundRetrieved(List<UserId> userIds) =>
                    $"Family was not retrieved using user ids {string.Join(",", userIds.Select(id => id.Value))}.";

                public static string UpdatedSuccessfully(FamilyId familyId) =>
                   $"Family {familyId.Value} was successfully updated.";

                public static string UpdatedSuccessfully(List<FamilyId> familyIds) =>
                   $"Families {string.Join(",", familyIds.Select(id => id.Value))} were successfully updated.";

                public static string SelectFamilyRoleRequest =>
                    $"{Constants.Messages.FormatChoosingTimeIntoString(AddToFamilyTimeoutInSeconds)} Please, select one of the roles:";

                public static class AddToFamily
                {
                    public static string SuccessfulConfirmation(FamilyRole familyRole)
                    {
                        StringBuilder sb = new();

                        sb.Append(string.Create(
                            CultureInfo.InvariantCulture,
                            $"You have selected {familyRole.GetDescription()} role."));

                        return sb.ToString();
                    }
                }
            }

            public static class ReplyKeys
            {
                public const string AddToFamily = "+family";
                public const string RemoveFromFamily = "-family";
            }

            public static class Costs
            {
                public const decimal AddChild = 70;
                public const decimal RemoveChild = 100;
                public const decimal AddPet = 20;
                public const decimal RemovePet = 30;
            }
        }
    }
}
