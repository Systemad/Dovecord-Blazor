﻿// <auto-generated />
using System;
using Dovecord.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dovecord.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211030140735_ChangeChannelName")]
    partial class ChangeChannelName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0-rc.2.21480.5");

            modelBuilder.Entity("Dovecord.Shared.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Channels");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9e999e73-1a25-4e40-bb4a-3013dbdeeff2"),
                            Name = "General"
                        },
                        new
                        {
                            Id = new Guid("58c1d9ab-a564-428e-aaba-af356a207193"),
                            Name = "Random"
                        });
                });

            modelBuilder.Entity("Dovecord.Shared.ChannelMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEdit")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("UserId");

                    b.ToTable("ChannelMessages");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c6bbe9c9-dd31-46c5-be80-fd8001f5cea7"),
                            ChannelId = new Guid("9e999e73-1a25-4e40-bb4a-3013dbdeeff2"),
                            Content = "First ever channel message",
                            CreatedAt = new DateTime(2021, 10, 30, 16, 7, 34, 792, DateTimeKind.Local).AddTicks(3145),
                            IsEdit = false,
                            UserId = new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"),
                            Username = "danova"
                        });
                });

            modelBuilder.Entity("Dovecord.Shared.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Online")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"),
                            Online = false,
                            Username = "danova"
                        });
                });

            modelBuilder.Entity("Dovecord.Shared.ChannelMessage", b =>
                {
                    b.HasOne("Dovecord.Shared.Channel", "Channel")
                        .WithMany("ChannelMessages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dovecord.Shared.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dovecord.Shared.User", b =>
                {
                    b.HasOne("Dovecord.Shared.User", null)
                        .WithMany("Users")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Dovecord.Shared.Channel", b =>
                {
                    b.Navigation("ChannelMessages");
                });

            modelBuilder.Entity("Dovecord.Shared.User", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
