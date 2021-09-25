using WMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Controllers
{
    public class WarehousesController : BaseReadController<Warehouse>
    {
        public WarehousesController(DbContext dbContext):
            base(dbContext) {}
    }
}
