using AutoMapper;
using WMS.BusinessLogic.DTO;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Mappers
{
    /// <summary>
    /// Automapper configuration
    /// </summary>
    public class WmsProfile : Profile
    {
        public WmsProfile()
        {
            CreateMap<ItemDto, Item>();
            // CreateMap<WarehouseDto, Warehouse>(); // only read, not necessary
            CreateMap<WarehouseItemDto, WarehouseItem>();
        }
    }
}
