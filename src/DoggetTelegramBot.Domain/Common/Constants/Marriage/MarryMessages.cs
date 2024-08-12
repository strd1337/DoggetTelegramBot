using System.Globalization;
using System.Text;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Domain.Common.Constants.Marriage
{
    public static partial class Constants
    {
        public static partial class Marriage
        {
            public static partial class Marry
            {
                public static class Messages
                {
                    public static string SelectMarriageTypeRequest =>
                       $"You have agreed to get married! Please, select one of the marriage types:";

                    public static string SuccessfulConfirmation(MarriageType marriageType)
                    {
                        StringBuilder sb = new();

                        sb.Append(string.Create(
                            CultureInfo.InvariantCulture,
                            $"You have selected {marriageType.GetDescription()} marriage type."));

                        return sb.ToString();
                    }
                }
            }
        }
    }
}
