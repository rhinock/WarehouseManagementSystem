using AutoMapper;
using WMS.BusinessLogic.DTO;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Mappers
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
