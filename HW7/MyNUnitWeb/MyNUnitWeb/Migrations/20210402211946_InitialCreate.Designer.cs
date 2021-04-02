﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyNUnitWeb.Models;

namespace MyNUnitWeb.Migrations
{
    [DbContext(typeof(Archive))]
    [Migration("20210402211946_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyNUnitWeb.Models.TestReportModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Passed")
                        .HasColumnType("bit");

                    b.Property<long?>("Time")
                        .HasColumnType("bigint");

                    b.Property<string>("WhyIgnored")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("reports");
                });
#pragma warning restore 612, 618
        }
    }
}
