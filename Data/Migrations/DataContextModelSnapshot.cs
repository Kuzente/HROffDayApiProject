﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Branch", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Core.Entities.BranchUser", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BranchID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("BranchID");

                    b.HasIndex("UserID");

                    b.ToTable("BranchUsers");
                });

            modelBuilder.Entity("Core.Entities.DailyFoodLog", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("AddedFoodAidAmount")
                        .HasColumnType("float");

                    b.Property<string>("AddedFoodAidAmountDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("DailyFoodLogs");
                });

            modelBuilder.Entity("Core.Entities.DailyYearLog", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AddedYearLeave")
                        .HasColumnType("int");

                    b.Property<string>("AddedYearLeaveDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("DailyYearLogs");
                });

            modelBuilder.Entity("Core.Entities.MissingDay", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Branch_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndOffDayDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Personal_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartJobDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartOffdayDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Personal_Id");

                    b.ToTable("MissingDays");
                });

            modelBuilder.Entity("Core.Entities.OffDay", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BranchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CountLeave")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DocumentNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeaveByDead")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByFather")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByFreeDay")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByMarried")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByPublicHoliday")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByTaken")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByTravel")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByWeek")
                        .HasColumnType("int");

                    b.Property<int>("LeaveByYear")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("OffDayStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("Personal_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PositionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Personal_Id");

                    b.ToTable("OffDays");
                });

            modelBuilder.Entity("Core.Entities.Personal", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Branch_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndJobDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FoodAid")
                        .HasColumnType("int");

                    b.Property<DateTime>("FoodAidDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBackToWork")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PersonalDetails_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Phonenumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Position_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RegistirationNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RetiredDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("RetiredOrOld")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartJobDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalTakenLeave")
                        .HasColumnType("int");

                    b.Property<int>("TotalYearLeave")
                        .HasColumnType("int");

                    b.Property<int>("UsedYearLeave")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Branch_Id");

                    b.HasIndex("Position_Id");

                    b.ToTable("Personals");
                });

            modelBuilder.Entity("Core.Entities.PersonalDetails", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BirthPlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BloodGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BodySize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EducationStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Handicapped")
                        .HasColumnType("bit");

                    b.Property<string>("IBAN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaritalStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("MotherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonalGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Personal_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.Property<string>("SgkCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SskNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Personal_Id")
                        .IsUnique();

                    b.ToTable("PersonalDetails");
                });

            modelBuilder.Entity("Core.Entities.Position", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Core.Entities.TransferPersonal", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewBranch")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewPosition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldBranch")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldPosition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Personal_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Personal_Id");

                    b.ToTable("TransferPersonals");
                });

            modelBuilder.Entity("Core.Entities.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDefaultPassword")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = new Guid("15db391f-8823-4b1a-8139-b5a979abb3fc"),
                            CreatedAt = new DateTime(2024, 3, 31, 16, 58, 16, 875, DateTimeKind.Local).AddTicks(5991),
                            DeletedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "superadmin@superadmin.com",
                            IsDefaultPassword = true,
                            ModifiedAt = new DateTime(2024, 3, 31, 16, 58, 16, 875, DateTimeKind.Local).AddTicks(6002),
                            Password = "superadmin",
                            Role = 0,
                            Status = 0,
                            Username = "superadmin"
                        });
                });

            modelBuilder.Entity("Core.Entities.BranchUser", b =>
                {
                    b.HasOne("Core.Entities.Branch", "Branch")
                        .WithMany("BranchUsers")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.User", "User")
                        .WithMany("BranchUsers")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Entities.MissingDay", b =>
                {
                    b.HasOne("Core.Entities.Personal", "Personal")
                        .WithMany("MissingDays")
                        .HasForeignKey("Personal_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Personal");
                });

            modelBuilder.Entity("Core.Entities.OffDay", b =>
                {
                    b.HasOne("Core.Entities.Personal", "Personal")
                        .WithMany("OffDays")
                        .HasForeignKey("Personal_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Personal");
                });

            modelBuilder.Entity("Core.Entities.Personal", b =>
                {
                    b.HasOne("Core.Entities.Branch", "Branch")
                        .WithMany("Personals")
                        .HasForeignKey("Branch_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Position", "Position")
                        .WithMany("Personals")
                        .HasForeignKey("Position_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("Core.Entities.PersonalDetails", b =>
                {
                    b.HasOne("Core.Entities.Personal", "Personal")
                        .WithOne("PersonalDetails")
                        .HasForeignKey("Core.Entities.PersonalDetails", "Personal_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Personal");
                });

            modelBuilder.Entity("Core.Entities.TransferPersonal", b =>
                {
                    b.HasOne("Core.Entities.Personal", "Personal")
                        .WithMany("TransferPersonals")
                        .HasForeignKey("Personal_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Personal");
                });

            modelBuilder.Entity("Core.Entities.Branch", b =>
                {
                    b.Navigation("BranchUsers");

                    b.Navigation("Personals");
                });

            modelBuilder.Entity("Core.Entities.Personal", b =>
                {
                    b.Navigation("MissingDays");

                    b.Navigation("OffDays");

                    b.Navigation("PersonalDetails")
                        .IsRequired();

                    b.Navigation("TransferPersonals");
                });

            modelBuilder.Entity("Core.Entities.Position", b =>
                {
                    b.Navigation("Personals");
                });

            modelBuilder.Entity("Core.Entities.User", b =>
                {
                    b.Navigation("BranchUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
