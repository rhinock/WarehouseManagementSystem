using System;
using WMS.DataAccess.Models;

namespace WMS.UI.Controllers
{
    public class WarehousesController : ReadControllerBase<Warehouse>
    {
        public WarehousesController(IServiceProvider serviceProvider)
            : base(serviceProvider) { }
    }
}
