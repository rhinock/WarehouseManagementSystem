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
        public DbSet<WarehouseItem> ItemWarehouses { get; set; }
        /// <summary>
        /// Начальные данные при создании модели
        /// </summary>
        /// <param name="modelBuilder"></param>
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
