using System;
using WMS.BusinessLogic.DTO;
using WMS.DataAccess.Models;

namespace WMS.UI.Controllers
{
    public class ItemsController : DataControllerBase<Item, ItemDto>
    {
        public ItemsController(IServiceProvider serviceProvider)
            : base(serviceProvider) { }
    }
}
