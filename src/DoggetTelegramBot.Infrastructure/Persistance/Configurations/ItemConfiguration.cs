using DoggetTelegramBot.Domain.Models.ItemEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoggetTelegramBot.Infrastructure.Persistance.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ItemId.Create(value));

            builder.Property(i => i.Name)
                .HasMaxLength(100);

            builder.Property(i => i.Description)
                .HasMaxLength(500);

            builder.Property(i => i.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Type)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
