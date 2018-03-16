﻿// <auto-generated />
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Data.Migrations
{
    [DbContext(typeof(BicycleShopContext))]
    [Migration("20180316191934_InitialSchema")]
    partial class InitialSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Entities.Bicycle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Discontinued")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<decimal>("Price");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Bicycle");
                });

            modelBuilder.Entity("Data.Entities.BicyclePart", b =>
                {
                    b.Property<long>("BicycleId");

                    b.Property<long>("PartId");

                    b.HasKey("BicycleId", "PartId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("PartId");

                    b.ToTable("BicyclePart");
                });

            modelBuilder.Entity("Data.Entities.Part", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Discontinued")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<decimal>("Price");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Part");
                });

            modelBuilder.Entity("Data.Entities.BicyclePart", b =>
                {
                    b.HasOne("Data.Entities.Bicycle", "Bicycle")
                        .WithMany("BicycleParts")
                        .HasForeignKey("BicycleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Entities.Part", "Part")
                        .WithMany("BicycleParts")
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
