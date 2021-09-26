namespace WMS.Models
{
    /// <summary>
    /// Содержание товара
    /// </summary>
    public class WarehouseItem : ModelBase
    {
        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public long WarehouseId { get; set; }
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public long ItemId { get; set; }
        /// <summary>
        /// Количество товара на складе
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// Склад
        /// </summary>
        public Warehouse Warehouse { get; set; }
        /// <summary>
        /// Товар
        /// </summary>
        public Item Item { get; set; }
    }
}
