namespace WMS.DataAccess.Models
{
    public class WarehouseItem : ModelBase
    {
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public long Count { get; set; }
        public Warehouse Warehouse { get; set; }
        public Item Item { get; set; }
    }
}
