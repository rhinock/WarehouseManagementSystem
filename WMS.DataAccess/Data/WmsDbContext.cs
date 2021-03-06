using Microsoft.EntityFrameworkCore;
using WMS.DataAccess.Models;

namespace WMS.DataAccess.Data
{
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        /// <summary>
        /// initial data
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Warehouses

            modelBuilder.Entity<Warehouse>().Property(w => w.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Warehouse>().HasIndex(w => w.Name).IsUnique();

            Warehouse[] warehouses = new Warehouse[]
            {
                new Warehouse { Id = 1, Name = "miniature", MaximumItems = 100 },
                new Warehouse { Id = 2, Name = "decent", MaximumItems = 10000 },
                new Warehouse { Id = 3, Name = "hefty", MaximumItems = 1000000 }
            };

            modelBuilder.Entity<Warehouse>().HasData(warehouses);

            // Items

            modelBuilder.Entity<Item>().Property(i => i.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Item>().HasIndex(i => i.Name).IsUnique();

            Item[] items = new Item[] {
                new Item { Id = 1, Name = "pencil", Price = 10.00m },
                new Item { Id = 2, Name = "pen", Price = 20.00m },
                new Item { Id = 3, Name = "felt-tip pen", Price = 30.00m }
                };

            modelBuilder.Entity<Item>().HasData(items);

            // WarehouseItems

            modelBuilder.Entity<WarehouseItem>().Property(wi => wi.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<WarehouseItem>()
                .HasIndex(wi => new { wi.WarehouseId, wi.ItemId } )
                .IsUnique();

            WarehouseItem[] warehouseItems = new WarehouseItem[] {
                new WarehouseItem
                {
                    Id = 1,
                    ItemId = items[0].Id,
                    Count = 50,
                    WarehouseId = warehouses[0].Id
                },
                new WarehouseItem
                {
                    Id = 2,
                    ItemId = items[1].Id,
                    Count = 5000,
                    WarehouseId = warehouses[1].Id
                },
                new WarehouseItem
                {
                    Id = 3,
                    ItemId = items[2].Id,
                    Count = 500000,
                    WarehouseId = warehouses[2].Id
                }
            };

            modelBuilder.Entity<WarehouseItem>().HasData(warehouseItems);
        }
    }
}
