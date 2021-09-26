using WMS.Models;
using Microsoft.EntityFrameworkCore;
using WMS.DTO;
using AutoMapper;
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
