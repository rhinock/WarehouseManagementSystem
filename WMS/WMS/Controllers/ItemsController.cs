using System;
using WMS.BusinessLogic.DTO;
using WMS.DataAccess.Models;

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
