using WMS.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace WMS.Controllers
{
    /// <summary>
    /// Контроллер для складов
    /// </summary>
    public class WarehousesController : ReadControllerBase<Warehouse>
    {
        public WarehousesController(IServiceProvider serviceProvider) :
            base(serviceProvider) {}
    }
}
