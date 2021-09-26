namespace WMS.DTO
{
    /// <summary>
    /// DTO для WarehouseItem (содержание товара на складе)
    /// </summary>
    public class WarehouseItemDto : IHaveId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
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
        // public long Count { get; set; }
    }
}
