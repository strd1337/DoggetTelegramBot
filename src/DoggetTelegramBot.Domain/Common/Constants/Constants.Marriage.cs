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

                public static string Retrieved(List<MarriageId> marriageIds) =>
                    $"Marriages {string.Join(",", marriageIds.Select(id => id.Value))} were retrieved.";

                public static string NotFoundRetrieved(UserId userId) =>
                    $"Marriages were not retrieved using user id {userId.Value}.";

                public static string NotFoundRetrieved(List<UserId> userIds) =>
                    $"Marriage was not retrieved using user ids {string.Join(",", userIds.Select(id => id.Value))}";

                public static string MarryRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Marry request started." :
                    $"Marry request ended.";

                public static string DivorceRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Divorce request started." :
                    $"Divorce request ended.";

                public static string ComposeMarryOrDivorceProposal(
                    string requesterFirstName,
                    string? requesterUsername,
                    string recipientFirstName,
                    string? recipientUsername,
                    bool isGetMarried = true)
                {
                    StringBuilder sb = new();
                    sb.Append(string.Create(
                        CultureInfo.InvariantCulture,
                        $"{(recipientUsername is not null ? $"@{recipientUsername}" : recipientFirstName)}, "));

                    if (isGetMarried)
                    {
                        sb.Append(string.Create(
                            CultureInfo.InvariantCulture,
                            $"do you want to get married to "));
                    }
                    else
                    {
                        sb.Append(string.Create(
                            CultureInfo.InvariantCulture,
                            $"do you want to divorce "));
                    }

                    sb.Append(string.Create(
                        CultureInfo.InvariantCulture,
                        $"{(requesterUsername is not null ? $"@{requesterUsername}" : requesterFirstName)}? "));

                    sb.Append("Time is limited.");

                    return sb.ToString();
                }

                public static string DenyMarryOrDivorceRequest(
                    string firstName,
                    string? username,
                    bool isGetMarried = true)
                {
                    StringBuilder sb = new();

                    sb.Append(username is not null ? $"@{username}" : firstName);
                    sb.Append(" has denied ");
                    sb.Append(isGetMarried ? "marrying request." : "divorcing request.");

                    return sb.ToString();
                }

                public static string UpdatedSuccessfully(MarriageId marriageId) =>
                   $"Marriage {marriageId.Value} was successfully updated.";
            }

            public static class ReplyKeys
            {
                public const string Marry = $"+marriage";
                public const string Divorce = $"+divorce";
            }
        }
    }
}
