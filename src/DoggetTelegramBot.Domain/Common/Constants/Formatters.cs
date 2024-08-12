using System.Text;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static partial class Formatters
        {
            public static string FormatChoosingTimeIntoString(int timeoutInSeconds = 10)
            {
                StringBuilder timeMessage = new();

                TimeSpan timeout = TimeSpan.FromSeconds(timeoutInSeconds);
                int minutes = timeout.Minutes;
                int seconds = timeout.Seconds;

                timeMessage = minutes > 0 ?
                    timeMessage.Append(string.Format(null, CompositeFormat.Parse($"{minutes} minute{(minutes > 1 ? "s" : "")}"))) :
                    timeMessage.Append(string.Empty);

                if (seconds > 0)
                {
                    if (timeMessage.Length != 0)
                    {
                        timeMessage.Append(" and ");
                    }
                    timeMessage.Append(string.Format(null, CompositeFormat.Parse($"{seconds} second{(seconds > 1 ? "s" : string.Empty)}")));
                }

                return $"The time for choosing is limited to {timeMessage}.";
            }

            public static string FormatCommandUsageTimeIntoString(TimeSpan time)
            {
                StringBuilder timeMessage = new();

                int days = time.Days;
                int hours = time.Hours;
                int minutes = time.Minutes;

                timeMessage = days > 0 ?
                    timeMessage.Append(string.Format(null, CompositeFormat.Parse($"{days} day{(days > 1 ? "s" : string.Empty)}"))) :
                    timeMessage.Append(string.Empty);

                if (hours > 0)
                {
                    if (timeMessage.Length != 0)
                    {
                        timeMessage.Append(" and ");
                    }
                    timeMessage.Append(string.Format(null, CompositeFormat.Parse($"{hours} hour{(hours > 1 ? "s" : string.Empty)}")));
                }

                if (minutes > 0)
                {
                    if (timeMessage.Length != 0)
                    {
                        timeMessage.Append(" and ");
                    }
                    timeMessage.Append(string.Format(null, CompositeFormat.Parse($"{minutes} minute{(minutes > 1 ? "s" : string.Empty)}")));
                }

                return timeMessage.ToString();
            }
        }
    }
}
