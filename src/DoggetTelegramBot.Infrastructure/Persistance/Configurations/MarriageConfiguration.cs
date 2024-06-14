using DoggetTelegramBot.Domain.Models.MarriageEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoggetTelegramBot.Infrastructure.Persistance.Configurations
{
    public class MarriageConfiguration : IEntityTypeConfiguration<Marriage>
    {
        public void Configure(EntityTypeBuilder<Marriage> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => MarriageId.Create(value));

            builder.Property(m => m.MarriageDate);

            builder.Property(m => m.DivorceDate);

            builder.Property(m => m.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(m => m.Status)
               .HasConversion<string>()
               .HasMaxLength(50);

            builder.OwnsMany(m => m.SpouseIds, sib =>
            {
                sib.ToTable("MarriageSpouseIds");

                sib.WithOwner().HasForeignKey("MarriageId");

                sib.HasKey("Id");

                sib.Property(si => si.Value)
                    .HasColumnName("UserId")
                    .ValueGeneratedNever();
            });
        }
    }
}
