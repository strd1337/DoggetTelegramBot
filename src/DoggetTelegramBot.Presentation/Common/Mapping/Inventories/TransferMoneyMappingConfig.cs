using System.Text;
using DoggetTelegramBot.Application.Inventories.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Inventories
{
    public sealed class TransferMoneyMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<TransferMoneyResult, Response>()
                .Map(dest => dest.Message, src => FormatTransferMoneyMessage(
                    src.Amount,
                    src.NewBalance));

        private static string FormatTransferMoneyMessage(
            decimal amount,
            decimal newBalance)
        {
            string yuanAmountLabel = amount > 1 ? "yuans" : "yuan";
            string yuanNewBalanceLabel = amount > 1 ? "yuans" : "yuan";

            StringBuilder sb = new();
            sb.Append($"You successfully transfer {amount} {yuanAmountLabel}. ");
            sb.Append($"Your new balance is {newBalance} {yuanNewBalanceLabel}.");
            return sb.ToString();
        }
    }
}
