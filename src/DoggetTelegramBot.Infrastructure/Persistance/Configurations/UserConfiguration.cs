using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoggetTelegramBot.Infrastructure.Persistance.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));

            builder.Property(u => u.TelegramId);

            builder.Property(u => u.Nickname)
                .HasMaxLength(100);

            builder.Property(u => u.FirstName)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasMaxLength(100);

            builder.Property(u => u.RegisteredDate);

            builder.Property(u => u.InventoryId)
                .HasConversion(
                    id => id.Value,
                    value => InventoryId.Create(value));

            builder.Property(u => u.MaritalStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(u => u.Privilege)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
