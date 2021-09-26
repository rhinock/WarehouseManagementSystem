using WMS.Models;
using Microsoft.EntityFrameworkCore;


namespace WMS.Controllers
{
    /// <summary>
    /// Контроллер для складов
    /// </summary>
    public class WarehousesController : BaseReadController<Warehouse>
    {
        public WarehousesController(DbContext dbContext):
            base(dbContext) {}
    }
}
