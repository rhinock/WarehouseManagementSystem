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
    public class ItemsService : ServiceBase<Item, ItemDto>
    {
        public ItemsService(IServiceProvider serviceProvider, DbContext dbContext, IMapper mapper) : 
            base(serviceProvider, dbContext, mapper)
        {
        }
        protected ValidationResult ValidateItemFields(ItemDto dto)
        {
            if (dto.Price < 0)
            {
                return BadResult("Price should be positive");
            }

            if (dto.Name.IsEmpty())
            {
                return BadResult("Name of the Item mustn't be empty");
            }

            bool isExist = _dbContext
                    .Set<Item>()
                    .Any(i => i.Name == dto.Name);

            if (isExist)
                return BadResult("Name of the Item must be unique");

            return ValidationResult.SuccessResult(BusinessResult.OK);
        }
        protected override ValidationResult ValidateBeforeInsert(ItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeInsert(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            return ValidateItemFields(dto);
        }
        protected override ValidationResult ValidateBeforeUpdate(ItemDto dto)
        {
            ValidationResult baseValidationResult = base.ValidateBeforeUpdate(dto);

            if (!baseValidationResult.Success)
            {
                return baseValidationResult;
            }

            return ValidateItemFields(dto);
        }
        
        private ValidationResult BadResult(string message)
        {
            return ValidationResult.FailureResult(BusinessResult.BadRequest, message);
        }
    }
}
