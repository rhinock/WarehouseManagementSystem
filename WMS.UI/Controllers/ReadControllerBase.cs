using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using WMS.DataAccess.Models;
using WMS.BusinessLogic.Services;
using WMS.BusinessLogic.Utils;
using WMS.BusinessLogic.Extensions;

namespace WMS.UI.Controllers
{
    /// <summary>
    /// read data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public class ReadControllerBase<T> : ControllerBase where T: ModelBase
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IReadService<T> _readService;
        public ReadControllerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _readService = _serviceProvider.GetService<IReadService<T>>();
        }
        /// <summary>
        /// Get Warehouses, Items or WarehouseItems
        /// </summary>
        /// <returns>Ok(result.Data)</returns>
        [HttpGet]
        public virtual IActionResult Get()
        {
            return HandleRequest(_readService.Get());
        }
        /// <summary>
        /// Get Warehouses, Items or WarehouseItems by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok(result.Data)</returns>
        [HttpGet("{id}")]
        public virtual IActionResult Get(long id)
        {
            return HandleRequest(_readService.Get(id));
        }

        public virtual IActionResult HandleRequest<TResult>(DataResult<TResult> dataResult)
        {
            switch (dataResult.BusinessResult)
            {
                case BusinessResult.Created:
                    if (dataResult.Message.IsEmpty())
                    {
                        return Created(dataResult.Url, dataResult.Data);
                    }
                    return new JsonResult(dataResult.Message ?? string.Empty)
                    {
                        StatusCode = StatusCodes.Status201Created
                    };

                case BusinessResult.NoContent:
                    if (dataResult.Message.IsEmpty())
                    {
                        return NoContent();
                    }
                    return new JsonResult(dataResult.Message ?? string.Empty)
                    {
                        StatusCode = StatusCodes.Status204NoContent
                    };

                case BusinessResult.BadRequest:
                    if (dataResult.Message.IsEmpty())
                    {
                        return BadRequest();
                    }
                    return new JsonResult(dataResult.Message ?? string.Empty)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                case BusinessResult.NotFound:
                    if (dataResult.Message.IsEmpty())
                    {
                        return NotFound();
                    }
                    return new JsonResult(dataResult.Message ?? string.Empty)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };

                default:
                    return Ok(dataResult.Data);
            }
        }
    }
}
