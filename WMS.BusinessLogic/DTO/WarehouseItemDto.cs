namespace WMS.BusinessLogic.DTO
{
    public class WarehouseItemDto : IHaveId
    {
        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public long Count { get; set; }
    }
}
