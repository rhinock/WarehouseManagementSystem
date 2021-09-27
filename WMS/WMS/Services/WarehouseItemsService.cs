using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS.DTO;
using WMS.Models;
using WMS.Utils;

namespace WMS.Services
{
    /// <summary>
    /// Сервис для товаров на складе
    /// </summary>
    public class WarehouseItemsService : ServiceBase<WarehouseItem, WarehouseItemDto>
    {
        public WarehouseItemsService(IServiceProvider serviceProvider, DbContext dbContext, IMapper mapper) 
            : base(serviceProvider, dbContext, mapper)
        {
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeInsert(WarehouseItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeInsert(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            if (dto.Count < 0)
            {
                //return ValidationResult.FailureResult(
                //    new JsonResult("Количество товара должно быть положительным числом")
                //    {
                //        StatusCode = StatusCodes.Status400BadRequest
                //    });
                return ValidationResult.FailureResult(
                    BusinessResult.BadRequest, "Количество товара должно быть положительным числом");
            }

            Warehouse warehouse = _dbContext.Set<Warehouse>()
                .FirstOrDefault(w => w.Id == dto.WarehouseId);

            if (warehouse == null)
                // return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
                return ValidationResult.FailureResult(BusinessResult.NotFound);

            Item item = _dbContext.Set<Item>()
                .FirstOrDefault(i => i.Id == dto.ItemId);

            if (item == null)
                // return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
                return ValidationResult.FailureResult(BusinessResult.NotFound);

            long? warehouseItemsCurrentCount =
                _dbContext
                    .Set<WarehouseItem>()
                    .Where(w => w.WarehouseId == dto.WarehouseId)
                    .Sum(c => c.Count);

            if (warehouseItemsCurrentCount != null)
            {
                if (dto.Count + warehouseItemsCurrentCount > warehouse.MaximumItems)
                {
                    // return ValidationResult.FailureResult(
                        //new JsonResult(
                        //    $"товар: {item.Name}, количество: {dto.Count}\\n" +
                        //    $"склад: {warehouse.Name}, максимальное количество товаров: {warehouse.MaximumItems}\\n" +
                        //    $"текущая заполненность склада: {warehouseItemsCurrentCount}\\n" +
                        //    $"добавляемое количество товара не должно превышать максимальное количество товаров")
                        //    {
                        //        StatusCode = StatusCodes.Status400BadRequest
                        //    });
                    return ValidationResult.FailureResult
                    (
                        BusinessResult.BadRequest,
                        $"товар: {item.Name}, количество: {dto.Count}\\n" +
                        $"склад: {warehouse.Name}, максимальное количество товаров: {warehouse.MaximumItems}\\n" +
                        $"текущая заполненность склада: {warehouseItemsCurrentCount}\\n" +
                        $"добавляемое количество товара не должно превышать максимальное количество товаров"
                    );
                }
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
                // Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = warehouseItem }
            };
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeUpdate(WarehouseItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeUpdate(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            if (dto.Count < 0)
            {
                // return ValidationResult.FailureResult(
                //new JsonResult("Количество товара должно быть положительным числом")
                //{
                //    StatusCode = StatusCodes.Status400BadRequest
                //});
                return ValidationResult.FailureResult
                    (BusinessResult.BadRequest, "Количество товара должно быть положительным числом");
            }

            WarehouseItem warehouseItem = baseValidationResult.EntityCache["entity"] as WarehouseItem;

            Warehouse warehouse = _dbContext.Set<Warehouse>()
                .FirstOrDefault(w => w.Id == dto.WarehouseId);

            if (warehouse == null)
                // return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
                return ValidationResult.FailureResult(BusinessResult.NotFound);


            Item item = _dbContext.Set<Item>()
                .FirstOrDefault(i => i.Id == dto.ItemId);

            if (item == null)
                // return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
                return ValidationResult.FailureResult(BusinessResult.NotFound);

            long? warehouseItemsCurrentCount =
                _dbContext
                    .Set<WarehouseItem>()
                    .Where(w => w.WarehouseId == dto.WarehouseId && w.Id != dto.Id)
                    .Sum(c => c.Count);

            if (warehouseItemsCurrentCount != null)
            {
                if (dto.Count + warehouseItemsCurrentCount > warehouse.MaximumItems)
                {
                    //return ValidationResult.FailureResult(
                    //new JsonResult(
                    //    $)
                    //{
                    //    StatusCode = StatusCodes.Status400BadRequest
                    //});

                    return ValidationResult.FailureResult
                        (
                            BusinessResult.BadRequest,
                            "товар: {item.Name}, количество: {dto.Count}\\n" +
                            $"склад: {warehouse.Name}, максимальное количество товаров: {warehouse.MaximumItems}\\n" +
                            $"текущая заполненность склада: {warehouseItemsCurrentCount}\\n" +
                            $"добавляемое количество товара не должно превышать максимальное количество товаров"
                        );
                }
            }

            warehouseItem.Count = dto.Count;

            return new ValidationResult()
            {
                // Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = warehouseItem }
            };
        }
    }
}
