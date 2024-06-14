using DoggetTelegramBot.Domain.Models.TransactionEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoggetTelegramBot.Infrastructure.Persistance.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => TransactionId.Create(value));

            builder.Property(t => t.FromUserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));

            builder.Property(t => t.ToUserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));

            builder.Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.OwnsMany(t => t.ItemIds, ib =>
            {
                ib.ToTable("TransactionItemIds");

                ib.WithOwner().HasForeignKey("TransactionId");

                ib.HasKey("Id");

                ib.Property(ni => ni.Value)
                    .HasColumnName("ItemId")
                    .ValueGeneratedNever();
            });
        }
    }
}
