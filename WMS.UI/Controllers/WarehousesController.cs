﻿using System;
using WMS.DataAccess.Models;

namespace WMS.UI.Controllers
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