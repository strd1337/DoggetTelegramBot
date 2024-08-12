namespace DoggetTelegramBot.Domain.Common.Constants.Family
{
    public static partial class Constants
    {
        public static partial class Family
        {
            public static class Requests
            {
                public static string Create(bool isStarted = true) =>
                    isStarted ?
                    $"Create a family request started." :
                    $"Create a family request ended.";

                public static string Delete(bool isStarted = true) =>
                    isStarted ?
                    $"Delete a family request started." :
                    $"Delete a family request ended.";

                public static string DeleteMemberFromAllFamilies(bool isStarted = true) =>
                    isStarted ?
                    $"Delete a member from all families request started." :
                    $"Delete a member from all families request ended.";

                public static string GetAllInformation(bool isStarted = true) =>
                    isStarted ?
                    $"Get all families information request started." :
                    $"Get all families information request ended.";

                public static string AddToFamily(bool isStarted = true) =>
                    isStarted ?
                    $"Add to family request started." :
                    $"Add to family request ended.";

                public static string RemoveFromFamily(bool isStarted = true) =>
                    isStarted ?
                    $"Remove from family request started." :
                    $"Remove from family request ended.";
            }
        }
    }
}
