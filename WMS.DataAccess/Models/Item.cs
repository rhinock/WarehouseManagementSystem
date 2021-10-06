using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.DataAccess.Models
{
    public class Item : ModelBase
    {
        public string Name { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
