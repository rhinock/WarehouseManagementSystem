﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WMS.DTO;
using WMS.Extensions;
using WMS.Models;
using WMS.Utils;

namespace WMS.Services
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
        /// <returns></returns>
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
        /// <returns></returns>
        private ValidationResult BadResult(string message)
        {
            return ValidationResult.FailureResult(BusinessResult.BadRequest, message);
        }
    }
}