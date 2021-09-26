using WMS.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WMS.DTO;
using AutoMapper;
using WMS.Utils;


namespace WMS.Controllers
{
    /// <summary>
    /// Базовый контроллер для записи и валидации данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public class BaseDataController<T, TDto> : BaseReadController<T> 
        where T : ModelBase, new()
        where TDto : IHaveId
    {
        /// <summary>
        /// Маппер для DTO
        /// </summary>
        private readonly IMapper _mapper;
        public BaseDataController(DbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Создание склада или товара
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult Insert(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeUpdate(dto);

            if (!validationResult.Success)
            {
                return validationResult.ErrorResult;
            }

            T entity = _mapper.Map<T>(dto);

            _dbContext.Add<T>(entity);
            _dbContext.SaveChanges();

            return Ok(entity);
        }
        /// <summary>
        /// Изменение склада или товара
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public virtual IActionResult Update(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeUpdate(dto);

            if(!validationResult.Success)
            {
                return validationResult.ErrorResult;
            }

            T entity = validationResult.EntityCache["entity"] as T;

            _mapper.Map<TDto, T>(dto, entity);
            
            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return NoContent();
        }
        /// <summary>
        /// Валидация перед созданием
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual ValidationResult ValidateBeforeInsert(TDto dto)
        {
            if (dto == null)
            {
                return ValidationResult.FailureResult(BadRequest());
            }

            if (dto.Id != default(long))
            {
                return ValidationResult.FailureResult(BadRequest("Id не должен присутствовать в запросе"));
            }

            return ValidationResult.SuccessResult();
        }
        /// <summary>
        /// Валидация перед изменением
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual ValidationResult ValidateBeforeUpdate(TDto dto)
        {
            if (dto == null)
            {
                return ValidationResult.FailureResult(BadRequest());
            }

            T entity = _dbContext.Find<T>(dto.Id);
                       
            
            if (entity == null)
            {
                return ValidationResult.FailureResult(NotFound());
            }

            return new ValidationResult()
            {
                Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = entity }
            };
        }
    }
}
