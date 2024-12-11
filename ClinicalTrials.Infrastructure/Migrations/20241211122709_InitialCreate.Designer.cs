﻿// <auto-generated />
using System;
using ClinicalTrials.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClinicalTrials.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20241211122709_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("trial")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClinicalTrials.Domain.ClinicalTrial", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getUtcDate()");

                    b.Property<int?>("DurationInDays")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Participants")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrialId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Created");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Created"));

                    b.ToTable("ClinicalTrials", "trial");
                });
#pragma warning restore 612, 618
        }
    }
}
