using WMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Data
{
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options)
            : base(options)
        {
            // Database.EnsureCreated();
        }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<WarehouseItem> ItemWarehouses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>().Property(w => w.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { Id = 1, Name = "Склад миниатюрный", MaximumItems = 100 },
                new Warehouse { Id = 2, Name = "Склад приличный", MaximumItems = 10000 },
                new Warehouse { Id = 3, Name = "Склад здоровенный", MaximumItems = 1000000 }
                );
        }
    }
}
