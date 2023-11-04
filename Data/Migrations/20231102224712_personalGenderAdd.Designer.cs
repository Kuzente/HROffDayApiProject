﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231102224712_personalGenderAdd")]
    partial class personalGenderAdd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Branch", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

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

            modelBuilder.Entity("Core.Entities.OffDay", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CountLeave")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<int>("Personal_Id")
                        .HasColumnType("int");

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
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Branch_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndJobDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phonenumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Position_Id")
                        .HasColumnType("int");

                    b.Property<string>("RegistirationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RetiredOrOld")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartJobDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalYearLeave")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Branch_Id");

                    b.HasIndex("Position_Id");

                    b.ToTable("Personals");
                });

            modelBuilder.Entity("Core.Entities.Position", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

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

            modelBuilder.Entity("Core.Entities.Branch", b =>
                {
                    b.Navigation("Personals");
                });

            modelBuilder.Entity("Core.Entities.Personal", b =>
                {
                    b.Navigation("OffDays");
                });

            modelBuilder.Entity("Core.Entities.Position", b =>
                {
                    b.Navigation("Personals");
                });
#pragma warning restore 612, 618
        }
    }
}
