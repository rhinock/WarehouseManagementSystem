using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.DataAccess.Models
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
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
