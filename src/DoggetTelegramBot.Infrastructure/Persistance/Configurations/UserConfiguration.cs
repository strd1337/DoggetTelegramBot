using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

            builder.Property(u => u.Username)
                .HasMaxLength(100);

            builder.Property(u => u.Nickname)
                .HasMaxLength(100);

            builder.Property(u => u.RegisteredDate);

            builder.Property(u => u.InventoryId)
                .HasConversion(
                    id => id.Value,
                    value => InventoryId.Create(value));

            builder.Property(u => u.MaritalStatus)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(u => u.Privileges)
                .HasConversion(
                    v => v.Select(p => (int)p).ToArray(),
                    v => v.Select(p => (UserPrivilege)p).ToList())
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyList<UserPrivilege>>(
                   (c1, c2) => c1!.SequenceEqual(c2!),
                   c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                   c => c.ToList()));
        }
    }
}
