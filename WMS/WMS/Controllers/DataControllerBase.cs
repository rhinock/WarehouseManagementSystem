using WMS.Models;
using Microsoft.AspNetCore.Mvc;
using WMS.DTO;
using System;
using WMS.Services;
using WMS.Extensions;

namespace WMS.Controllers
{
    /// <summary>
    /// Базовый контроллер для записи и валидации данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public class DataControllerBase<T, TDto> : ReadControllerBase<T>
        where T : ModelBase, new()
        where TDto : IHaveId
    {
        protected readonly IDataService<T, TDto> _dataService;
        public DataControllerBase(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            _dataService = serviceProvider.Get<IDataService<T, TDto>>();
        }
        /// <summary>
        /// Создание склада или товара
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Ok(result.Data)</returns>
        [HttpPost]
        public virtual IActionResult Insert(TDto dto)
        {
            var a = this.HttpContext.Request.Path;
            return HandleRequest(_dataService.Insert(dto));
        }
        /// <summary>
        /// Изменение склада или товара
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>NoContent()</returns>
        [HttpPut]
        public virtual IActionResult Update(TDto dto)
        {
            return HandleRequest(_dataService.Update(dto));
        }
    }
}
