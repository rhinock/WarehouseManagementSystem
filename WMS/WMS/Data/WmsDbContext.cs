using WMS.Models;
using Microsoft.EntityFrameworkCore;


namespace WMS.Data
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options)
            : base(options)
        {
            // Database.EnsureCreated();
        }
        /// <summary>
        /// Таблица складов
        /// </summary>
        public DbSet<Warehouse> Warehouses { get; set; }
        /// <summary>
        /// Таблица товаров
        /// </summary>
        public DbSet<Item> Items { get; set; }
        /// <summary>
        /// Таблица содержания товаров на складе
        /// </summary>
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        /// <summary>
        /// Начальные данные при создании модели
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Склады

            modelBuilder.Entity<Warehouse>().Property(w => w.Id).ValueGeneratedOnAdd();

            Warehouse[] warehouses = new Warehouse[]
            {
                new Warehouse { Id = 1, Name = "Склад миниатюрный", MaximumItems = 100 },
                new Warehouse { Id = 2, Name = "Склад приличный", MaximumItems = 10000 },
                new Warehouse { Id = 3, Name = "Склад здоровенный", MaximumItems = 1000000 }
            };

            modelBuilder.Entity<Warehouse>().HasData(warehouses);

            // Товары

            modelBuilder.Entity<Item>().Property(i => i.Id).ValueGeneratedOnAdd();

            Item[] items = new Item[] {
                new Item { Id = 1, Name = "Карандаш", Price = 10.00m },
                new Item { Id = 2, Name = "Ручка", Price = 20.00m },
                new Item { Id = 3, Name = "Фломастер", Price = 30.00m }
                };

            modelBuilder.Entity<Item>().HasData(items);
        }
    }
}
