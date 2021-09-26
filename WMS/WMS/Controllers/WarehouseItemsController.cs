using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS.DTO;
using WMS.Models;
using WMS.Utils;


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
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var result = _readService.Get(id);

            if (!result.Success)
            {
                return result.ErrorResult;
            }

            return NoContent(); 
        }
    }
}
