using WMS.Models;
using Microsoft.EntityFrameworkCore;
using WMS.DTO;
// using WMS.Mappers;
using AutoMapper;

namespace WMS.Controllers
{
    public class ItemsController : BaseDataController<Item, ItemDto>
    {
        // public ItemsController(DbContext dbContext, BaseMapper<ItemDto, Item> mapper)
        public ItemsController(DbContext dbContext, IMapper mapper)
            : base(dbContext, mapper) { }
    }
}
