using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Enums;
using WMS.BusinessLogic.Utils;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Services
{
    public class WarehouseItemsService : ServiceBase<WarehouseItem, WarehouseItemDto>
    {
        public WarehouseItemsService(IServiceProvider serviceProvider, DbContext dbContext, IMapper mapper)
            : base(serviceProvider, dbContext, mapper)
        {
        }
        protected ValidationResult ValidateWarehouseItemFields(
            OperationType operationType, WarehouseItemDto warehouseItemDto)
        {
            if (warehouseItemDto.Count < 0)
            {
                return ValidationResult.FailureResult
                    (BusinessResult.BadRequest, "Count should be positive");
            }

            Warehouse warehouse = _dbContext.Set<Warehouse>()
                .FirstOrDefault(w => w.Id == warehouseItemDto.WarehouseId);

            if (warehouse == null)
                return ValidationResult.FailureResult
                    (BusinessResult.NotFound, "WarehouseId wasn't provided");

            Item item = _dbContext.Set<Item>()
                .FirstOrDefault(i => i.Id == warehouseItemDto.ItemId);

            if (item == null)
                return ValidationResult.FailureResult
                    (BusinessResult.NotFound, "ItemId wasn't provided");

            WarehouseItem warehouseItem = null;

            switch (operationType)
            {
                case OperationType.Insert:

                    bool isExists = _dbContext.Set<WarehouseItem>().
                        Any(wi => wi.WarehouseId == warehouseItemDto.WarehouseId &&
                            wi.ItemId == warehouseItemDto.ItemId);

                    if (isExists)
                    {
                        return ValidationResult.FailureResult
                        (
                            BusinessResult.BadRequest,
                            $"Item: {item.Name} already exists in warehouse: {warehouse.Name}"
                        );
                    }

                    warehouseItem = new WarehouseItem
                    {
                        Item = item,
                        ItemId = item.Id,
                        Warehouse = warehouse,
                        WarehouseId = warehouse.Id,
                        Count = warehouseItemDto.Count
                    };

                    break;

                case OperationType.Update:

                    warehouseItem = _dbContext.Find<WarehouseItem>(warehouseItemDto.Id);
                    warehouseItemDto.Count = warehouseItemDto.Count;
                    break;
            }

            long? warehouseItemsCurrentCount = null;

            switch (operationType)
            {
                case OperationType.Insert:
                    warehouseItemsCurrentCount = _dbContext
                        .Set<WarehouseItem>()
                        .Where(w => w.WarehouseId == warehouseItemDto.WarehouseId)
                        .Sum(c => c.Count);
                    break;
                case OperationType.Update:
                    warehouseItemsCurrentCount = _dbContext
                        .Set<WarehouseItem>()
                        .Where(w => w.WarehouseId == warehouseItemDto.WarehouseId && w.Id != warehouseItemDto.Id)
                        .Sum(c => c.Count);
                    break;
            }

            if (warehouseItemsCurrentCount != null)
            {
                if (warehouseItemDto.Count + warehouseItemsCurrentCount > warehouse.MaximumItems)
                {
                    return ValidationResult.FailureResult
                        (
                            BusinessResult.BadRequest,
                            $"item: {item.Name}, count: {warehouseItemDto.Count}\\n" +
                            $"warehouse: {warehouse.Name}, maximum items: { warehouse.MaximumItems}\\n" +
                            $"current count of warehouse: {warehouseItemsCurrentCount}\\n" +
                            $"the added count of items must not exceed the maximum count of items"
                        );
                }
            }

            return new ValidationResult()
            {
                Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = warehouseItem }
            };
        }
        protected override ValidationResult ValidateBeforeInsert(WarehouseItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeInsert(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            return ValidateWarehouseItemFields(OperationType.Insert, dto);
        }
        protected override ValidationResult ValidateBeforeUpdate(WarehouseItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeUpdate(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            return ValidateWarehouseItemFields(OperationType.Update, dto);
        }
    }
}
