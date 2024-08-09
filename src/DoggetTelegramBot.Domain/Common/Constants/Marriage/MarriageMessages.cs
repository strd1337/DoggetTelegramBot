using System.Globalization;
using System.Text;
using Formatters = DoggetTelegramBot.Domain.Common.Constants.Constants.Formatters;

namespace DoggetTelegramBot.Domain.Common.Constants.Marriage
{
    public static partial class Constants
    {
        public static partial class Marriage
        {
            public static class Messages
            {
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

                    sb.Append(Formatters.FormatChoosingTimeIntoString());

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
            }
        }
    }
}
