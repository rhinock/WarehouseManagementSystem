using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeInsert(WarehouseItemDto dto)
        {
            // return base.ValidateBeforeInsert(dto);

            ValidationResult baseValidationResult = base.ValidateBeforeInsert(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            Warehouse warehouse = _dbContext.Set<Warehouse>()
                .FirstOrDefault(w => w.Id == dto.WarehouseId);

            if (warehouse == null)
                return ValidationResult.FailureResult(NotFound());

            Item item = _dbContext.Set<Item>()
                .FirstOrDefault(i => i.Id == dto.ItemId);

            if (item == null)
                return ValidationResult.FailureResult(NotFound());

            long? warehouseItemsCurrentCount =
                _dbContext
                    .Set<WarehouseItem>()
                    .Where(w => w.WarehouseId == dto.WarehouseId)
                    .Sum(c => c.Count);

            if (warehouseItemsCurrentCount != null)
            {
                if (dto.Count + warehouseItemsCurrentCount > warehouse.MaximumItems)
                    return ValidationResult.FailureResult(
                        BadRequest(
                            $"товар: {item.Name}, количество: {dto.Count}\n" +
                            $"склад: {warehouse.Name}, максимальное количество товаров: {warehouse.MaximumItems}\n" +
                            $"текущая заполненность склада: {warehouseItemsCurrentCount}\n" +
                            $"добавляемое количество товара не должно превышать максимальное количество товаров"
                            ));
            }

            WarehouseItem warehouseItem = new WarehouseItem
            {
                Item = item,
                ItemId = item.Id,
                Warehouse = warehouse,
                WarehouseId = warehouse.Id,
                Count = dto.Count
            };

            return new ValidationResult()
            {
                Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = warehouseItem }
            };
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeUpdate(WarehouseItemDto dto)
        {
            // return base.ValidateBeforeUpdate(dto);

            ValidationResult baseValidationResult = base.ValidateBeforeUpdate(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            WarehouseItem warehouseItem = baseValidationResult.EntityCache["entity"] as WarehouseItem;

            Warehouse warehouse = _dbContext.Set<Warehouse>()
                .FirstOrDefault(w => w.Id == dto.WarehouseId);

            if (warehouse == null)
                return ValidationResult.FailureResult(NotFound());

            Item item = _dbContext.Set<Item>()
                .FirstOrDefault(i => i.Id == dto.ItemId);

            if (item == null)
                return ValidationResult.FailureResult(NotFound());

            long? warehouseItemsCurrentCount =
                _dbContext
                    .Set<WarehouseItem>()
                    .Where(w => w.WarehouseId == dto.WarehouseId && w.Id != dto.Id)
                    .Sum(c => c.Count);

            if (warehouseItemsCurrentCount != null)
            {
                if (dto.Count + warehouseItemsCurrentCount > warehouse.MaximumItems)
                    return ValidationResult.FailureResult(
                        BadRequest(
                            $"товар: {item.Name}, количество: {dto.Count}\n" +
                            $"склад: {warehouse.Name}, максимальное количество товаров: {warehouse.MaximumItems}\n" +
                            $"текущая заполненность склада: {warehouseItemsCurrentCount}\n" +
                            $"добавляемое количество товара не должно превышать максимальное количество товаров"
                            ));
            }

            warehouseItem.Count = dto.Count;

            return new ValidationResult()
            {
                Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = warehouseItem }
            };
        }
    }
}
