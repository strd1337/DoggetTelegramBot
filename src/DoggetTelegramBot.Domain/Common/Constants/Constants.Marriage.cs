using System.Globalization;
using System.Text;
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

                public static string ComposeMarriageProposal(
                    string requesterFirstName,
                    string? requesterUsername,
                    string recipientFirstName,
                    string? recipientUsername)
                {
                    StringBuilder sb = new();
                    sb.Append(string.Create(
                        CultureInfo.InvariantCulture,
                        $"@{recipientUsername ?? recipientFirstName}, "));

                    sb.Append(string.Create(
                        CultureInfo.InvariantCulture,
                        $"do you want to get married to "));

                    sb.Append(string.Create(
                        CultureInfo.InvariantCulture,
                        $"{requesterUsername ?? requesterFirstName}? "));

                    sb.Append("Time is limited.");

                    return sb.ToString();
                }

                public static string DenyMarriageRequest(string firstName, string? username) =>
                    $"@{username ?? firstName} has denied the marriage request.";

                public const string NotFoundUserReply =
                   $"Select the user and reply on his message using the command: {ReplyKeys.CreateMarriage}";
            }

            public static class ReplyKeys
            {
                public const string CreateMarriage = $"+marriage";
            }
        }
    }
}
