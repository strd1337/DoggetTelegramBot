using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using PRTelegramBot.Extensions;
using System.Globalization;
using System.Text;
using Formatters = DoggetTelegramBot.Domain.Common.Constants.Constants.Formatters;

namespace DoggetTelegramBot.Domain.Common.Constants.Family
{
    public static partial class Constants
    {
        public static partial class Family
        {
            public static partial class AddTo
            {
                public static class Messages
                {
                    public static string SelectFamilyRoleRequest =>
                        $"{Formatters.FormatChoosingTimeIntoString(AddToFamilyTimeoutInSeconds)} Please, select one of the roles:";

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
        }
    }
}
