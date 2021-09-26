using WMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WMS.Utils;
using System;
using WMS.Services;
using WMS.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace WMS.Controllers
{
    /// <summary>
    /// Базовый контроллер для чтения данных
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
            // _readService = _serviceProvider.Get<IReadService<T>>();
            _readService = _serviceProvider.GetService<IReadService<T>>();
        }
        /// <summary>
        /// Получить весь список элементов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult Get()
        {
            var result = _readService.Get();

            if (!result.Success)
            {
                return result.ErrorResult;
            }

            return Ok(result.Data);
        }
        /// <summary>
        /// Получить список элементов по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual IActionResult Get(long id)
        {
            var result = _readService.Get(id);

            if (!result.Success)
            {
                return result.ErrorResult;
            }

            return Ok(result.Data);
        }
    }
}
