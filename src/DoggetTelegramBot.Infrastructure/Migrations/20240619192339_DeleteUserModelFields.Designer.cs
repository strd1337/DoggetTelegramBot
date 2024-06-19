// <auto-generated />
using System;
using DoggetTelegramBot.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DoggetTelegramBot.Infrastructure.Migrations
{
    [DbContext(typeof(BotDbContext))]
    [Migration("20240619192339_DeleteUserModelFields")]
    partial class DeleteUserModelFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.FamilyEntity.Family", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Families");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.InventoryEntity.Inventory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("YuanBalance")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.ItemEntity.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.MarriageEntity.Marriage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DivorceDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("MarriageDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Marriages");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.TransactionEntity.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FromUserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ToUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.UserEntity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("InventoryId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("MaritalStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int[]>("Privileges")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<DateTime>("RegisteredDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.FamilyEntity.Family", b =>
                {
                    b.OwnsMany("DoggetTelegramBot.Domain.Models.FamilyEntity.FamilyMember", "FamilyMembers", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasColumnName("FamilyMemberId");

                            b1.Property<Guid>("FamilyId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("CreatedDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<bool>("IsDeleted")
                                .HasColumnType("boolean");

                            b1.Property<DateTime>("ModifiedDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("Role")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.HasKey("Id", "FamilyId");

                            b1.HasIndex("FamilyId");

                            b1.ToTable("FamilyMembers", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("FamilyId");
                        });

                    b.Navigation("FamilyMembers");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.InventoryEntity.Inventory", b =>
                {
                    b.OwnsMany("DoggetTelegramBot.Domain.Models.ItemEntity.ItemId", "ItemIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("InventoryId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("ItemId");

                            b1.HasKey("Id");

                            b1.HasIndex("InventoryId");

                            b1.ToTable("InventoryItemIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("InventoryId");
                        });

                    b.Navigation("ItemIds");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.MarriageEntity.Marriage", b =>
                {
                    b.OwnsMany("DoggetTelegramBot.Domain.Models.UserEntity.UserId", "SpouseIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("MarriageId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("UserId");

                            b1.HasKey("Id");

                            b1.HasIndex("MarriageId");

                            b1.ToTable("MarriageSpouseIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("MarriageId");
                        });

                    b.Navigation("SpouseIds");
                });

            modelBuilder.Entity("DoggetTelegramBot.Domain.Models.TransactionEntity.Transaction", b =>
                {
                    b.OwnsMany("DoggetTelegramBot.Domain.Models.ItemEntity.ItemId", "ItemIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("TransactionId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("ItemId");

                            b1.HasKey("Id");

                            b1.HasIndex("TransactionId");

                            b1.ToTable("TransactionItemIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("TransactionId");
                        });

                    b.Navigation("ItemIds");
                });
#pragma warning restore 612, 618
        }
    }
}
