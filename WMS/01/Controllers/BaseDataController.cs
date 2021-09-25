using WMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WMS.DTO;
using AutoMapper;
using WMS.Utils;
// using WMS.Mappers;

namespace WMS.Controllers
{
    public class BaseDataController<T, TDto> : BaseReadController<T> 
        where T : ModelBase, new()
        where TDto : IHaveId
    {
        // private readonly BaseMapper<TDto, T> _mapper;
        private readonly IMapper _mapper;
        // public BaseDataController(DbContext dbContext, BaseMapper<TDto, T> mapper) : base(dbContext)
        public BaseDataController(DbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
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
        [HttpPut]
        public virtual IActionResult Update(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeUpdate(dto);

            if(!validationResult.Success)
            {
                return validationResult.ErrorResult;
            }

            T entity = validationResult.EntityCache["entity"] as T;

            // _mapper.Convert(dto, entity);
            _mapper.Map<TDto, T>(dto, entity);
            
            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return NoContent();
        }

        protected virtual ValidationResult ValidateBeforeInsert(TDto dto)
        {
            if (dto == null)
            {
                // BadRequest();
                return ValidationResult.FailureResult(BadRequest());
            }

            if (dto.Id != default(long))
            {
                // BadRequest("Id не должен присутствовать в запросе");
                return ValidationResult.FailureResult(BadRequest("Id не должен присутствовать в запросе"));
            }

            // return Ok();
            return ValidationResult.SuccessResult();
        }

        protected virtual ValidationResult ValidateBeforeUpdate(TDto dto)
        {
            if (dto == null)
            {
                return ValidationResult.FailureResult(BadRequest());
            }

            T entity = _dbContext.Find<T>(dto.Id);
                       
            
            if (entity == null)
            {
                // return NotFound();
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
