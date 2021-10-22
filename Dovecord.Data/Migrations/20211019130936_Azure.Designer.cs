﻿// <auto-generated />
using System;
using Dovecord.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dovecord.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211019130936_Azure")]
    partial class Azure
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("Dovecord.Shared.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChannelName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Channels");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6ea179da-31bb-4a07-adbd-3f5713a1f8dd"),
                            ChannelName = "General"
                        },
                        new
                        {
                            Id = new Guid("1757fdac-99de-4a2b-9032-4604d948aeab"),
                            ChannelName = "Random"
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
                            Id = new Guid("249a1060-2257-4dba-8e8b-12aedf1f3349"),
                            ChannelId = new Guid("6ea179da-31bb-4a07-adbd-3f5713a1f8dd"),
                            Content = "First ever channel message",
                            CreatedAt = new DateTime(2021, 10, 19, 15, 9, 35, 553, DateTimeKind.Local).AddTicks(5850),
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