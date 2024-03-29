﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradingJournal.Infrastructure.Persistence;

#nullable disable

namespace TradingJournal.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220303121457_added hidden field")]
    partial class addedhiddenfield
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TradingJournal.Domain.Entities.Execution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Action")
                        .HasColumnType("int");

                    b.Property<int>("Direction")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExecutedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Fee")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("NetReturn")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("Position")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("Return")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("ReturnPercentage")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("Size")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<int>("TradeId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.HasKey("Id");

                    b.HasIndex("TradeId");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.Symbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.Trade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("AverageEntryPrice")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("AverageExitPrice")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<DateTime>("ClosedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Confluences")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<decimal>("NetReturn")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpenedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Position")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("Return")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("ReturnPercentage")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.Property<decimal>("Size")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("SymbolId")
                        .HasColumnType("int");

                    b.Property<int>("TradingAccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SymbolId");

                    b.HasIndex("TradingAccountId");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.TradingAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("APIKey")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("APISecret")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TradingAccounts");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.Execution", b =>
                {
                    b.HasOne("TradingJournal.Domain.Entities.Trade", "Trade")
                        .WithMany("Executions")
                        .HasForeignKey("TradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trade");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.Trade", b =>
                {
                    b.HasOne("TradingJournal.Domain.Entities.Symbol", "Symbol")
                        .WithMany()
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TradingJournal.Domain.Entities.TradingAccount", "TradingAccount")
                        .WithMany()
                        .HasForeignKey("TradingAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Symbol");

                    b.Navigation("TradingAccount");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.TradingAccount", b =>
                {
                    b.HasOne("TradingJournal.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TradingJournal.Domain.Entities.Trade", b =>
                {
                    b.Navigation("Executions");
                });
#pragma warning restore 612, 618
        }
    }
}
