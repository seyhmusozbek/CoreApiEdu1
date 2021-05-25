﻿// <auto-generated />
using System;
using CoreApiEdu1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreApiEdu1.Migrations
{
    [DbContext(typeof(BarcodeContext))]
    [Migration("20210525185054_addcarts")]
    partial class addcarts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreApiEdu1.Entities.Cart", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("productIdid")
                        .HasColumnType("int");

                    b.Property<double>("quantity")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.HasIndex("productIdid");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("CoreApiEdu1.Entities.Product", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("imageUrl")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("isFavorite")
                        .HasColumnType("bit");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<string>("title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CoreApiEdu1.Entities.Cart", b =>
                {
                    b.HasOne("CoreApiEdu1.Entities.Product", "productId")
                        .WithMany()
                        .HasForeignKey("productIdid");

                    b.Navigation("productId");
                });
#pragma warning restore 612, 618
        }
    }
}
