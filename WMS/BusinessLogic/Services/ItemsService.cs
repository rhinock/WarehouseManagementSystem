using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Extensions;
using WMS.BusinessLogic.Utils;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Services
{
    /// <summary>
    /// Сервис товаров
    /// </summary>
    public class ItemsService : ServiceBase<Item, ItemDto>
    {
        public ItemsService(IServiceProvider serviceProvider, DbContext dbContext, IMapper mapper) : 
            base(serviceProvider, dbContext, mapper)
        {
        }
        /// <summary>
        /// Валидация столбцов товара
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>ValidationResult</returns>
        protected ValidationResult ValidateItemFields(ItemDto dto)
        {
            if (dto.Price < 0)
            {
                return BadResult("Цена должна быть положительным числом");
            }

            if (dto.Name.IsEmpty())
            {
                return BadResult("Имя товара не должно быть пустым");
            }

            bool isExist = _dbContext
                    .Set<Item>()
                    .Any(i => i.Name == dto.Name);

            if (isExist)
                return BadResult("Имя товара должно быть уникальным");

            return ValidationResult.SuccessResult(BusinessResult.OK);
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeInsert(ItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeInsert(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            return ValidateItemFields(dto);
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeUpdate(ItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeUpdate(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            return ValidateItemFields(dto);
        }
        /// <summary>
        /// Кастомизированный вывод ошибки с сообщением
        /// </summary>
        /// <param name="message"></param>
        /// <returns>ValidationResult</returns>
        private ValidationResult BadResult(string message)
        {
            return ValidationResult.FailureResult(BusinessResult.BadRequest, message);
        }
    }
}
