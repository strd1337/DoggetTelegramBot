using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace DoggetTelegramBot.Infrastructure.Persistance
{
    public class BotDbContext(
        DbContextOptions<BotDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(
                    typeof(BotDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Marriage> Marriages { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Inventory> Inventories { get; set; } = null!;
        public DbSet<Family> Families { get; set; } = null!;
        public DbSet<FamilyMember> FamilyMembers { get; set; } = null!;
    }
}
