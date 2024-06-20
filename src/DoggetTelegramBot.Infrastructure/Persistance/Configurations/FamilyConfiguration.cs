using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoggetTelegramBot.Infrastructure.Persistance.Configurations
{
    public class FamilyConfiguration : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => FamilyId.Create(value));

            builder.OwnsMany(f => f.Members, fmb =>
            {
                fmb.ToTable("FamilyMembers");

                fmb.WithOwner().HasForeignKey("FamilyId");

                fmb.HasKey("Id", "FamilyId");

                fmb.Property(fm => fm.Id)
                    .HasColumnName("FamilyMemberId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => FamilyMemberId.Create(value));

                fmb.Property(fm => fm.UserId)
                    .HasConversion(
                        id => id.Value,
                        value => UserId.Create(value));

                fmb.Property(fm => fm.Role)
                    .HasConversion<string>()
                    .HasMaxLength(50);
            });

            builder.Metadata.FindNavigation(nameof(Family.Members))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
