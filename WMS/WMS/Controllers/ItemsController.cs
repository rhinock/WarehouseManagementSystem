using WMS.Models;
using WMS.DTO;
using System;

namespace WMS.Controllers
{
    /// <summary>
    /// Контроллер для товаров
    /// </summary>
    public class ItemsController : DataControllerBase<Item, ItemDto>
    {
        public ItemsController(IServiceProvider serviceProvider)
            : base(serviceProvider) { }
    }
}
