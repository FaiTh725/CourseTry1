﻿// <auto-generated />
using System;
using CourseTry1.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CourseTry1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240330172905_RemoveNessesary")]
    partial class RemoveNessesary
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CourseTry1.Domain.Entity.DayWeek", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<long>("SheduleGroupId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SheduleGroupId");

                    b.ToTable("DayWeeks");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.ExcelFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.Profile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.SheduleGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("NameGroup")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ProfileId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("SheduleGroups");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.Subject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("DayWeekId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DayWeekId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.DayWeek", b =>
                {
                    b.HasOne("CourseTry1.Domain.Entity.SheduleGroup", "SheduleGroup")
                        .WithMany("Weeks")
                        .HasForeignKey("SheduleGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SheduleGroup");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.Profile", b =>
                {
                    b.HasOne("CourseTry1.Domain.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.SheduleGroup", b =>
                {
                    b.HasOne("CourseTry1.Domain.Entity.Profile", null)
                        .WithMany("Groups")
                        .HasForeignKey("ProfileId");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.Subject", b =>
                {
                    b.HasOne("CourseTry1.Domain.Entity.DayWeek", "DayWeek")
                        .WithMany("PairingTime")
                        .HasForeignKey("DayWeekId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DayWeek");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.DayWeek", b =>
                {
                    b.Navigation("PairingTime");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.Profile", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("CourseTry1.Domain.Entity.SheduleGroup", b =>
                {
                    b.Navigation("Weeks");
                });
#pragma warning restore 612, 618
        }
    }
}
