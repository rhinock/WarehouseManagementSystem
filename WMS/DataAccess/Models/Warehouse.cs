namespace WMS.DataAccess.Models
{
    /// <summary>
    /// Склад
    /// </summary>
    public class Warehouse : ModelBase
    {
        /// <summary>
        /// Имя склада
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Максимальное количество товаров на складе
        /// </summary>
        public long MaximumItems { get; set; }
    }
}
