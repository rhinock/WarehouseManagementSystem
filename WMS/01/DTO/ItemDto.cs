namespace WMS.DTO
{
    /// <summary>
    /// DTO (Data Tranfser Object) для Item (товар)</see>
    /// </summary>
    public class ItemDto : IHaveId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Имя товара
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; }
    }
}
