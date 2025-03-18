﻿// <auto-generated />
using System;
using FashionVote.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FashionVote.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("FashionVote.Models.Designer", b =>
                {
                    b.Property<int>("DesignerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("DesignerId");

                    b.ToTable("Designers");
                });

            modelBuilder.Entity("FashionVote.Models.DesignerShow", b =>
                {
                    b.Property<int>("DesignerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShowId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DesignerId", "ShowId");

                    b.HasIndex("ShowId");

                    b.ToTable("DesignerShows");
                });

            modelBuilder.Entity("FashionVote.Models.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ShowId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ParticipantId");

                    b.HasIndex("ShowId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("FashionVote.Models.ParticipantShow", b =>
                {
                    b.Property<int>("ParticipantId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShowId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ParticipantId", "ShowId");

                    b.HasIndex("ShowId");

                    b.ToTable("ParticipantShows");
                });

            modelBuilder.Entity("FashionVote.Models.Show", b =>
                {
                    b.Property<int>("ShowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShowName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("ShowId");

                    b.ToTable("Shows");
                });

            modelBuilder.Entity("FashionVote.Models.Vote", b =>
                {
                    b.Property<int>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DesignerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("ParticipantId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShowId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("VotedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("VoteId");

                    b.HasIndex("DesignerId");

                    b.HasIndex("ShowId");

                    b.HasIndex("ParticipantId", "DesignerId", "ShowId")
                        .IsUnique();

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FashionVote.Models.DesignerShow", b =>
                {
                    b.HasOne("FashionVote.Models.Designer", "Designer")
                        .WithMany("DesignerShows")
                        .HasForeignKey("DesignerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FashionVote.Models.Show", "Show")
                        .WithMany("DesignerShows")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Designer");

                    b.Navigation("Show");
                });

            modelBuilder.Entity("FashionVote.Models.Participant", b =>
                {
                    b.HasOne("FashionVote.Models.Show", "Show")
                        .WithMany("Participants")
                        .HasForeignKey("ShowId");

                    b.Navigation("Show");
                });

            modelBuilder.Entity("FashionVote.Models.ParticipantShow", b =>
                {
                    b.HasOne("FashionVote.Models.Participant", "Participant")
                        .WithMany("ParticipantShows")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FashionVote.Models.Show", "Show")
                        .WithMany("ParticipantShows")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant");

                    b.Navigation("Show");
                });

            modelBuilder.Entity("FashionVote.Models.Vote", b =>
                {
                    b.HasOne("FashionVote.Models.Designer", "Designer")
                        .WithMany("Votes")
                        .HasForeignKey("DesignerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FashionVote.Models.Participant", "Participant")
                        .WithMany("Votes")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FashionVote.Models.Show", "Show")
                        .WithMany("Votes")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Designer");

                    b.Navigation("Participant");

                    b.Navigation("Show");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FashionVote.Models.Designer", b =>
                {
                    b.Navigation("DesignerShows");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("FashionVote.Models.Participant", b =>
                {
                    b.Navigation("ParticipantShows");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("FashionVote.Models.Show", b =>
                {
                    b.Navigation("DesignerShows");

                    b.Navigation("ParticipantShows");

                    b.Navigation("Participants");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
