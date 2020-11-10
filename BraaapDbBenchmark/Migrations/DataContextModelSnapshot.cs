﻿// <auto-generated />
using System;
using BraaapDbBenchmark.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BraaapDbBenchmark.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BraaapDbBenchmark.Model.Checkpoint", b =>
                {
                    b.Property<Guid>("CheckpointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("RiderId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("SessionId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("CheckpointId");

                    b.ToTable("Checkpoints");
                });

            modelBuilder.Entity("BraaapDbBenchmark.Model.Rider", b =>
                {
                    b.Property<Guid>("RiderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Number")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("RiderId");

                    b.ToTable("Riders");
                });

            modelBuilder.Entity("BraaapDbBenchmark.Model.RiderSessionResult", b =>
                {
                    b.Property<Guid>("RiderSessionResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<Guid?>("RiderId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RiderName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RiderNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid?>("SessionId")
                        .HasColumnType("char(36)");

                    b.HasKey("RiderSessionResultId");

                    b.ToTable("RiderSessionResults");
                });

            modelBuilder.Entity("BraaapDbBenchmark.Model.Session", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("SessionId");

                    b.ToTable("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}