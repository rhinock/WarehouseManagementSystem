using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Utils;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Services
{
    public class ServiceBase<T, TDto> : IDataService<T, TDto>
        where T : ModelBase
        where TDto: IHaveId
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly DbContext _dbContext;
        protected readonly IMapper _mapper;

        public ServiceBase(IServiceProvider serviceProvider, DbContext dbContext, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public virtual DataResult<IEnumerable<T>> Get()
        {
            return DataResult<IEnumerable<T>>.SuccessResult(BusinessResult.OK, _dbContext.GetAll<T>(true).ToList());
        }
        public virtual DataResult<T> Get(long id)
        {
            T entity = _dbContext.GetAll<T>(true).FirstOrDefault(w => w.Id == id);
            
            if (entity == null)
            {
                return DataResult<T>.FailureResult(BusinessResult.NotFound);
            }

            return DataResult<T>.SuccessResult(BusinessResult.OK, entity);
        }
        public virtual DataResult<T> Insert(TDto dto)
        {
            ValidationResult validationResult = ValidateBeforeInsert(dto);

            if (!validationResult.Success)
            {
                return DataResult<T>.FailureResult(validationResult.BusinessResult, validationResult.Message);
            }

            T entity = _mapper.Map<T>(dto);

            _dbContext.Add<T>(entity);
            _dbContext.SaveChanges();

            return DataResult<T>.SuccessResult(BusinessResult.Created, entity);
        }
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
        protected virtual ValidationResult ValidateBeforeInsert(TDto dto)
        {
            if (dto == null)
            {
                return ValidationResult.FailureResult
                    (BusinessResult.BadRequest, "The object mustn't be empty");
            }

            if (dto.Id != default(long))
            {
                return ValidationResult.FailureResult
                    (BusinessResult.BadRequest, "Id mustn't be in body");
            }

            return ValidationResult.SuccessResult(BusinessResult.OK);
        }
        protected virtual ValidationResult ValidateBeforeUpdate(TDto dto)
        {
            if (dto == null)
            {
                return ValidationResult.FailureResult(BusinessResult.BadRequest);
            }

            T entity = _dbContext.Find<T>(dto.Id);

            if (entity == null)
            {
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
