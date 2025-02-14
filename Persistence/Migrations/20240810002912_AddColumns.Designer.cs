﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(SQLContext))]
    [Migration("20240810002912_AddColumns")]
    partial class AddColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.6.24327.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Entities.Card", b =>
                {
                    b.Property<string>("Number")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<short>("CVV")
                        .HasColumnType("smallint");

                    b.Property<string>("Holder")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("Valid")
                        .HasColumnType("date");

                    b.HasKey("Number");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Cards", (string)null);
                });

            modelBuilder.Entity("Domain.Models.Entities.Product", b =>
                {
                    b.Property<int>("Article")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Article"));

                    b.Property<int>("AlreadyCooked")
                        .HasColumnType("int");

                    b.Property<int>("AlreadyRated")
                        .HasColumnType("int");

                    b.Property<TimeOnly?>("AverageCookingTime")
                        .HasColumnType("time");

                    b.Property<string>("ImageName")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<float?>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<bool>("Visible")
                        .HasColumnType("bit");

                    b.HasKey("Article");

                    b.HasIndex("Article")
                        .IsUnique();

                    b.ToTable("Goods", (string)null);
                });

            modelBuilder.Entity("Domain.Models.Entities.Restaurant", b =>
                {
                    b.Property<string>("Adress")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Adress");

                    b.HasIndex("Adress")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("Password")
                        .IsUnique();

                    b.ToTable("Restaurants", (string)null);
                });

            modelBuilder.Entity("Domain.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<decimal>("Bonuses")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)")
                        .HasColumnName("_Login");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)")
                        .HasColumnName("_Name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)")
                        .HasColumnName("_Password");

                    b.Property<string>("TelegramId")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("Password")
                        .IsUnique();

                    b.HasIndex("TelegramId")
                        .IsUnique()
                        .HasFilter("[TelegramId] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.Models.Entities.Card", b =>
                {
                    b.HasOne("Domain.Models.Entities.User", "User")
                        .WithMany("Cards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.Entities.User", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
