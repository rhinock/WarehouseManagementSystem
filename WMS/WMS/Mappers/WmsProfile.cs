using AutoMapper;
using WMS.DTO;
using WMS.Models;

namespace WMS.Mappers
{
    /// <summary>
    /// Настройка маппинга
    /// </summary>
    public class WmsProfile : Profile
    {
        public WmsProfile()
        {
            CreateMap<ItemDto, Item>();
            // CreateMap<WarehouseDto, Warehouse>(); // только чтение
            CreateMap<WarehouseItemDto, WarehouseItem>();
        }
    }
}
