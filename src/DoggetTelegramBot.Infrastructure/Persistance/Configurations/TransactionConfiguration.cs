using DoggetTelegramBot.Domain.Models.TransactionEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoggetTelegramBot.Infrastructure.Persistance.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.TransactionId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => TransactionId.Create(value));

            builder.Property(t => t.FromUserIds)
                .HasConversion(
                    v => string.Join(',', v.Select(id => id.Value.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(value => UserId.Create(Guid.Parse(value)))
                          .ToList())
                .Metadata.SetValueComparer(new ValueComparer<List<UserId>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            builder.Property(t => t.ToUserId)
                .HasConversion(
                    id => id!.Value,
                    value => UserId.Create(value));

            builder.Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Type)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
