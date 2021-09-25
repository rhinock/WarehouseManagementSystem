namespace WMS.Models
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Item : ModelBase
    {
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
