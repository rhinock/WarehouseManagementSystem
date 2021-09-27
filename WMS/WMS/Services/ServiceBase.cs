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
    /// базовый сервис
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
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

        public ServiceBase(IServiceProvider serviceProvider, DbContext dbContext, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// Получить данные
        /// </summary>
        /// <returns>DataResult<IEnumerable<T>>.SuccessResult(_dbContext.GetAll<T>(true).ToList())</returns>
        public virtual DataResult<IEnumerable<T>> Get()
        {
            return DataResult<IEnumerable<T>>.SuccessResult(BusinessResult.OK, _dbContext.GetAll<T>(true).ToList());
        }
        /// <summary>
        /// Получить данные по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DataResult<T>.SuccessResult(entity)</returns>
        public virtual DataResult<T> Get(long id)
        {
            T entity = _dbContext.GetAll<T>(true).FirstOrDefault(w => w.Id == id);
            // DataResult<T> dataResult = new DataResult<T>();

            if (entity == null)
            {
                // return DataResult<T>.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
                return DataResult<T>.FailureResult(BusinessResult.NotFound);
            }

            return DataResult<T>.SuccessResult(BusinessResult.OK, entity);
        }
        /// <summary>
        /// Добавить данные
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>DataResult<T>.SuccessResult(entity)</returns>
        public virtual DataResult<T> Insert(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeInsert(dto);
            // DataResult<T> dataResult = new DataResult<T>();

            if (!validationResult.Success)
            {
                return DataResult<T>.FailureResult(validationResult.BusinessResult, validationResult.Message);
                //dataResult.Success = validationResult.Success;
                //dataResult.ErrorResult = validationResult.ErrorResult;
            }

            T entity = _mapper.Map<T>(dto);

            _dbContext.Add<T>(entity);
            _dbContext.SaveChanges();

            return DataResult<T>.SuccessResult(BusinessResult.Created, entity);
            //dataResult.Data = entity;
            //return dataResult;
        }
        /// <summary>
        /// Обновить данные
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>DataResult<T>.SuccessResult()</returns>
        public virtual DataResult<T> Update(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeUpdate(dto);

            if (!validationResult.Success)
            {
                return DataResult<T>.FailureResult(validationResult.BusinessResult);
            }

            T entity = _dbContext.Set<T>().Single(e => e.Id == dto.Id);

            _mapper.Map<TDto, T>(dto, entity);

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return DataResult<T>.SuccessResult(BusinessResult.NoContent);
        }
        /// <summary>
        /// Удалить данные
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DataResult<T>.SuccessResult()</returns>
        public DataResult<T> Delete(long id)
        {
            WarehouseItem entity = _dbContext.Set<WarehouseItem>().FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                return DataResult<T>.FailureResult(BusinessResult.NotFound);
            }

            _dbContext.Remove<WarehouseItem>(entity);
            _dbContext.SaveChanges();

            return DataResult<T>.SuccessResult(BusinessResult.OK);
        }
        /// <summary>
        /// Валидация перед созданием
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>ValidationResult.SuccessResult()</returns>
        protected virtual ValidationResult ValidateBeforeInsert(TDto dto)
        {
            // ValidationResult validationResult = new ValidationResult();

            if (dto == null)
            {
                return ValidationResult.FailureResult(BusinessResult.BadRequest);
            }

            if (dto.Id != default(long))
            {
                return ValidationResult.FailureResult(BusinessResult.BadRequest, "Id не должен присутствовать в запросе");

                //return ValidationResult.FailureResult(
                //    new JsonResult("Id не должен присутствовать в запросе")
                //    {
                //        StatusCode = StatusCodes.Status400BadRequest
                //    });
            }

            return ValidationResult.SuccessResult(BusinessResult.OK);
            // return validationResult;
        }
        /// <summary>
        /// Валидация перед изменением
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>ValidationResult</returns>
        protected virtual ValidationResult ValidateBeforeUpdate(TDto dto)
        {
            if (dto == null)
            {
                // return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status400BadRequest));
                return ValidationResult.FailureResult(BusinessResult.BadRequest);
            }

            T entity = _dbContext.Find<T>(dto.Id);

            if (entity == null)
            {
                // return ValidationResult.FailureResult(new StatusCodeResult(StatusCodes.Status404NotFound));
                return ValidationResult.FailureResult(BusinessResult.NotFound);
            }

            return new ValidationResult()
            {
                Success = true,
                EntityCache = new Dictionary<string, ModelBase>() { ["entity"] = entity }
            };
        }
    }
}
