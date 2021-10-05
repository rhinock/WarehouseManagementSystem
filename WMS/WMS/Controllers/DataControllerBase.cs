using Microsoft.AspNetCore.Mvc;
using System;
using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Extensions;
using WMS.BusinessLogic.Services;
using WMS.DataAccess.Models;

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
            var dataResult = _dataService.Insert(dto);
            dataResult.Url = HttpContext.Request.Path;
            return HandleRequest(dataResult);
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
