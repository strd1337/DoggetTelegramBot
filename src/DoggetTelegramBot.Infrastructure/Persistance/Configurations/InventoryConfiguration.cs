using DoggetTelegramBot.Domain.Models.InventoryEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoggetTelegramBot.Infrastructure.Persistance.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.InventoryId);

            builder.Property(i => i.InventoryId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => InventoryId.Create(value));

            builder.Property(i => i.YuanBalance)
                .HasColumnType("decimal(18,2)");
        }
    }
}
