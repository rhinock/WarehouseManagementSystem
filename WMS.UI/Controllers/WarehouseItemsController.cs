using Microsoft.AspNetCore.Mvc;
using System;
using WMS.BusinessLogic.DTO;
using WMS.DataAccess.Models;

namespace WMS.UI.Controllers
{
    public class WarehouseItemsController : DataControllerBase<WarehouseItem, WarehouseItemDto>
    {
        public WarehouseItemsController(IServiceProvider serviceProvider) : base(serviceProvider) {}

        /// <summary>
        /// Delete WarehouseItem by Id
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
