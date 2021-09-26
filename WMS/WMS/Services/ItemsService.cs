using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public ItemsService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeInsert(ItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeInsert(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            if (dto.Price < 0)
            {
                return ReturnBadRequest("Цена должна быть положительным числом");
            }

            if (dto.Name.IsEmpty())
            {
                return ReturnBadRequest("Имя товара не должно быть пустым");
            }

            return ValidationResult.SuccessResult();
        }
        /// <inheritdoc />
        protected override ValidationResult ValidateBeforeUpdate(ItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeUpdate(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            if (dto.Price < 0)
            {
                return ReturnBadRequest("Цена должна быть положительным числом");
            }

            if (dto.Name.IsEmpty())
            {
                return ReturnBadRequest("Имя товара не должно быть пустым");
            }

            return ValidationResult.SuccessResult();
        }
        /// <summary>
        /// Кастомизированный вывод ошибки с сообщением
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private ValidationResult ReturnBadRequest(string message)
        {
            return ValidationResult.FailureResult(
                new JsonResult(message)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                });
        }
    }
}
