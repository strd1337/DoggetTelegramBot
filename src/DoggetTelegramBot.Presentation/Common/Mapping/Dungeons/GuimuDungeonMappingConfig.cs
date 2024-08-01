using System.Text;
using DoggetTelegramBot.Application.Dungeons.Guimu.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Subterraneans
{
    public sealed class GuimuDungeonMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
           config.NewConfig<GuimuDungeonResult, Response>()
               .Map(dest => dest.Message, src => FormatGuimuEndMessage(
                   src.Amount,
                   src.IsSpecialCase,
                   src.IsPositive));

        public string FormatGuimuEndMessage(
            decimal amount,
            bool isSpecialCase,
            bool isPositive)
        {
            StringBuilder sb = new();

            if (isSpecialCase)
            {
                sb.AppendLine($"You have got a lucky and won a significant amount of {amount} yuan. You can also you the command again!");
            }

            if (isPositive && !isSpecialCase)
            {
                sb.Append($"He has successfully conquered the Guimu dungeon");
                sb.AppendLine(amount > 0 ?
                    $" and won an impressive amount of {amount} yuan." :
                    $", but won {amount} yuan.");
            }

            if (!isPositive)
            {
                sb.AppendLine($"Unfortunately, he encountered setbacks and lost a total of {amount} yuan.");
            }

            return sb.ToString();
        }
    }
}
