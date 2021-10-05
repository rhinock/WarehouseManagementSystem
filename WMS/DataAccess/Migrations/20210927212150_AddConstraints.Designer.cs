﻿//// <auto-generated />
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//using WMS.Data;

//namespace WMS.Migrations
//{
//    [DbContext(typeof(WmsDbContext))]
//    [Migration("20210927212150_AddConstraints")]
//    partial class AddConstraints
//    {
//        protected override void BuildTargetModel(ModelBuilder modelBuilder)
//        {
//#pragma warning disable 612, 618
//            modelBuilder
//                .HasAnnotation("ProductVersion", "3.1.18")
//                .HasAnnotation("Relational:MaxIdentifierLength", 128)
//                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

//            modelBuilder.Entity("WMS.Models.Item", b =>
//                {
//                    b.Property<long>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("bigint")
//                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

//                    b.Property<string>("Name")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<decimal>("Price")
//                        .HasColumnType("decimal(18, 2)");

//                    b.HasKey("Id");

//                    b.HasIndex("Name")
//                        .IsUnique()
//                        .HasFilter("[Name] IS NOT NULL");

//                    b.ToTable("Items");

//                    b.HasData(
//                        new
//                        {
//                            Id = 1L,
//                            Name = "Карандаш",
//                            Price = 10.00m
//                        },
//                        new
//                        {
//                            Id = 2L,
//                            Name = "Ручка",
//                            Price = 20.00m
//                        },
//                        new
//                        {
//                            Id = 3L,
//                            Name = "Фломастер",
//                            Price = 30.00m
//                        });
//                });

//            modelBuilder.Entity("WMS.Models.Warehouse", b =>
//                {
//                    b.Property<long>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("bigint")
//                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

//                    b.Property<long>("MaximumItems")
//                        .HasColumnType("bigint");

//                    b.Property<string>("Name")
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("Name")
//                        .IsUnique()
//                        .HasFilter("[Name] IS NOT NULL");

//                    b.ToTable("Warehouses");

//                    b.HasData(
//                        new
//                        {
//                            Id = 1L,
//                            MaximumItems = 100L,
//                            Name = "Склад миниатюрный"
//                        },
//                        new
//                        {
//                            Id = 2L,
//                            MaximumItems = 10000L,
//                            Name = "Склад приличный"
//                        },
//                        new
//                        {
//                            Id = 3L,
//                            MaximumItems = 1000000L,
//                            Name = "Склад здоровенный"
//                        });
//                });

//            modelBuilder.Entity("WMS.Models.WarehouseItem", b =>
//                {
//                    b.Property<long>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("bigint")
//                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

//                    b.Property<long>("Count")
//                        .HasColumnType("bigint");

//                    b.Property<long>("ItemId")
//                        .HasColumnType("bigint");

//                    b.Property<long>("WarehouseId")
//                        .HasColumnType("bigint");

//                    b.HasKey("Id");

//                    b.HasIndex("ItemId");

//                    b.HasIndex("WarehouseId", "ItemId")
//                        .IsUnique();

//                    b.ToTable("WarehouseItems");

//                    b.HasData(
//                        new
//                        {
//                            Id = 1L,
//                            Count = 50L,
//                            ItemId = 1L,
//                            WarehouseId = 1L
//                        },
//                        new
//                        {
//                            Id = 2L,
//                            Count = 5000L,
//                            ItemId = 2L,
//                            WarehouseId = 2L
//                        },
//                        new
//                        {
//                            Id = 3L,
//                            Count = 500000L,
//                            ItemId = 3L,
//                            WarehouseId = 3L
//                        });
//                });

//            modelBuilder.Entity("WMS.Models.WarehouseItem", b =>
//                {
//                    b.HasOne("WMS.Models.Item", "Item")
//                        .WithMany()
//                        .HasForeignKey("ItemId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.HasOne("WMS.Models.Warehouse", "Warehouse")
//                        .WithMany()
//                        .HasForeignKey("WarehouseId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });
//#pragma warning restore 612, 618
//        }
//    }
//}
