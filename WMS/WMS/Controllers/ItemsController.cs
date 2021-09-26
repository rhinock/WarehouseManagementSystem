using WMS.Models;
using Microsoft.EntityFrameworkCore;
using WMS.DTO;
using AutoMapper;


namespace WMS.Controllers
{
    /// <summary>
    /// Контроллер для товаров
    /// </summary>
    public class ItemsController : BaseDataController<Item, ItemDto>
    {
        public ItemsController(DbContext dbContext, IMapper mapper)
            : base(dbContext, mapper) { }
    }
}
