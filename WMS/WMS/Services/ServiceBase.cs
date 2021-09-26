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
    public class ServiceBase<T, TDto> : IDataService<T, TDto>
        where T : ModelBase
        where TDto: IHaveId
    {
        /// <summary>
        /// IoC (Inversion of Control) -- инверсия контроля
        /// </summary>
        protected readonly IServiceProvider _serviceProvider;
        protected readonly DbContext _dbContext;
        protected readonly IMapper _mapper;

        public ServiceBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = _serviceProvider.GetService(typeof(DbContext)) as DbContext;
            _mapper = _serviceProvider.GetService(typeof(IMapper)) as IMapper;
        }

        public virtual DataResult<IEnumerable<T>> Get()
        {
            return DataResult<IEnumerable<T>>.SuccessResult(_dbContext.GetAll<T>(true).ToList());
        }
        public virtual DataResult<T> Get(long id)
        {
            T entity = _dbContext.GetAll<T>(true).FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                return DataResult<T>.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
            }

            return DataResult<T>.SuccessResult(entity);
        }
        public virtual DataResult<T> Insert(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeInsert(dto);

            if (!validationResult.Success)
            {
                return DataResult<T>.FailureResult(validationResult.ErrorResult);
            }

            T entity = _mapper.Map<T>(dto);

            _dbContext.Add<T>(entity);
            _dbContext.SaveChanges();

            return DataResult<T>.SuccessResult(entity);
        }
        public virtual DataResult<T> Update(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeUpdate(dto);

            if (!validationResult.Success)
            {
                return DataResult<T>.FailureResult(validationResult.ErrorResult);
            }

            T entity = _dbContext.Set<T>().Single(e => e.Id == dto.Id);

            _mapper.Map<TDto, T>(dto, entity);

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return DataResult<T>.SuccessResult();
        }
        public DataResult<T> Delete(long id)
        {
            WarehouseItem entity = _dbContext.Set<WarehouseItem>().FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                return DataResult<T>.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
            }

            _dbContext.Remove<WarehouseItem>(entity);
            _dbContext.SaveChanges();

            return DataResult<T>.SuccessResult();
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
                return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status400BadRequest));
            }

            if (dto.Id != default(long))
            {
                return ValidationResult.FailureResult(
                    new JsonResult("Id не должен присутствовать в запросе")
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    });
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
                return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status400BadRequest));
            }

            T entity = _dbContext.Find<T>(dto.Id);

            if (entity == null)
            {
                return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
            }

            return new ValidationResult()
            {
                Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = entity }
            };
        }
        
    }
}
