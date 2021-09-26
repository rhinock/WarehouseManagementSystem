using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WMS.DTO;
using WMS.Models;
using WMS.Utils;


namespace WMS.Controllers
{
    /// <summary>
    /// Контроллер для содержания товаров на складе
    /// </summary>
    public class WarehouseItemsController : BaseDataController<WarehouseItem, WarehouseItemDto>
    {
        public WarehouseItemsController(DbContext dbContext, IMapper mapper) : base(dbContext, mapper) {}

        /// <summary>
        /// Удаление товара со склада
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            WarehouseItem entity = _dbContext.Set<WarehouseItem>().FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            _dbContext.Remove<WarehouseItem>(entity);
            _dbContext.SaveChanges();

            return NoContent();
        }
        // TODO: add beforeupdate, beforeinsert
        protected override ValidationResult ValidateBeforeInsert(WarehouseItemDto dto)
        {
            return base.ValidateBeforeInsert(dto);
        }
        protected override ValidationResult ValidateBeforeUpdate(WarehouseItemDto dto)
        {
            return base.ValidateBeforeUpdate(dto);
        }
    }
}
