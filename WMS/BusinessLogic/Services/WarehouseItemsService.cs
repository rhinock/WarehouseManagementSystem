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
    /// <summary>
    /// Сервис для товаров на складе
    /// </summary>
    public class WarehouseItemsService : ServiceBase<WarehouseItem, WarehouseItemDto>
    {
        public WarehouseItemsService(IServiceProvider serviceProvider, DbContext dbContext, IMapper mapper)
            : base(serviceProvider, dbContext, mapper)
        {
        }
        /// <summary>
        /// Валидация столбцов товара
        /// </summary>
        /// <param name="warehouseItemDto"></param>
        /// <param name="operationType"></param>
        /// <returns>ValidationResult</returns>
        protected ValidationResult ValidateWarehouseItemFields(
            OperationType operationType, WarehouseItemDto warehouseItemDto)
        {
            if (warehouseItemDto.Count < 0)
            {
                return ValidationResult.FailureResult
                    (BusinessResult.BadRequest, "Количество товара должно быть положительным числом");
            }

            Warehouse warehouse = _dbContext.Set<Warehouse>()
                .FirstOrDefault(w => w.Id == warehouseItemDto.WarehouseId);

            if (warehouse == null)
                return ValidationResult.FailureResult(BusinessResult.NotFound, "Не указан идентификатор склада");

            Item item = _dbContext.Set<Item>()
                .FirstOrDefault(i => i.Id == warehouseItemDto.ItemId);

            if (item == null)
                return ValidationResult.FailureResult(BusinessResult.NotFound, "Не указан идентификатор товара");

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
                            $"товар: {item.Name} уже существует на складе: {warehouse.Name}"
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
                            $"товар: {item.Name}, количество: {warehouseItemDto.Count}\\n" +
                            $"склад: {warehouse.Name}, максимальное количество товаров: {warehouse.MaximumItems}\\n" +
                            $"текущая заполненность склада: {warehouseItemsCurrentCount}\\n" +
                            $"добавляемое количество товара не должно превышать максимальное количество товаров"
                        );
                }
            }

            return new ValidationResult()
            {
                Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = warehouseItem }
            };
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeInsert(WarehouseItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeInsert(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            return ValidateWarehouseItemFields(OperationType.Insert, dto);
        }
        /// <inheritdoc />
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
