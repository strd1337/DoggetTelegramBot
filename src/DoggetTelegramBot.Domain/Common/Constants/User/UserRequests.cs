namespace DoggetTelegramBot.Domain.Common.Constants.User
{
    public static partial class Constants
    {
        public static partial class User
        {
            public static class Requests
            {
                public static string GetInformation(bool isStarted = true) =>
                    isStarted ?
                    "Get user information request started." :
                    "Get user information request ended.";

                public static string Get(bool isStarted = true) =>
                    isStarted ?
                    "Get user request started." :
                    "Get user request ended.";

                public static string Update(bool isStarted = true) =>
                    isStarted ?
                    "Update user request started." :
                    "Update user request ended.";

                public static string Delete(bool isStarted = true) =>
                    isStarted ?
                    "Delete user request started." :
                    "Delete user request ended.";


                public static string CheckPrivilege(bool isStarted = true) =>
                    isStarted ?
                    "Check user privilege request started." :
                    "Check user privilege request ended.";

                public static string CheckExistence(bool isStarted = true) =>
                    isStarted ?
                    "Check user existence request started." :
                    "Check user existence request ended.";


                public static string GetTransactionParticipants(bool isStarted = true) =>
                    isStarted ?
                    "Get transaction participants request started." :
                    "Get transaction participants request ended.";

                public static string GetSpouses(bool isStarted = true) =>
                    isStarted ?
                    "Get spouses request started." :
                    "Get spouses request ended.";


                public static string UpdateNickname(bool isStarted = true) =>
                    isStarted ?
                    "Update nickname request started." :
                    "Update nickname request ended.";

                public static string UpdateMaritalStatus(bool isStarted = true) =>
                    isStarted ?
                    "Update marital status request started." :
                    "Update marital status request ended.";


                public static string ValidMessageReward(bool isStarted = true) =>
                    isStarted ?
                    "Valid user message reward request started." :
                    "Valid user message reward request ended.";

                public static string AddNewChatMember(bool isStarted = true) =>
                    isStarted ?
                    "Add new chat member request started." :
                    "Add new chat member request ended.";

                public static string ChatMemberLeft(bool isStarted = true) =>
                    isStarted ?
                    "Chat member left request started." :
                    "Chat member left request ended.";

                public static string SendChoice(bool isAgreed = true) =>
                    isAgreed ?
                    "You have agreed." :
                    "You have not agreed";
            }
        }
    }
}
