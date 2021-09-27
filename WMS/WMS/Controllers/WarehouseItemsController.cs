using Microsoft.AspNetCore.Mvc;
using System;
using WMS.DTO;
using WMS.Models;

namespace WMS.Controllers
{
    /// <summary>
    /// Контроллер для содержания товаров на складе
    /// </summary>
    public class WarehouseItemsController : DataControllerBase<WarehouseItem, WarehouseItemDto>
    {
        public WarehouseItemsController(IServiceProvider serviceProvider) : base(serviceProvider) {}

        /// <summary>
        /// Удаление товара со склада
        /// </summary>
        /// <param name="id"></param>
        /// <returns>NoContent()</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return HandleRequest(_dataService.Delete(id));
        }
    }
}
