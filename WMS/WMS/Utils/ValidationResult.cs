using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WMS.Models;


namespace WMS.Utils
{
    /// <summary>
    /// Класс для валидации результата
    /// </summary>
    public class ValidationResult
    {
        public bool Success { get; set; }
        public IActionResult ErrorResult { get; set; }
        /// <summary>
        /// Кэш для сущностей
        /// </summary>
        public IDictionary<string, ModelBase> EntityCache { get; set; }
        /// <summary>
        /// Успешный результат валидации
        /// </summary>
        /// <param name="entityCache"></param>
        /// <returns></returns>
        public static ValidationResult SuccessResult(IDictionary<string, ModelBase> entityCache = null)
        {
            return new ValidationResult { Success = true, EntityCache = entityCache, ErrorResult = null };
        }
        /// <summary>
        /// Неуспешный результат валидации
        /// </summary>
        /// <param name="errorResult"></param>
        /// <param name="entityCache"></param>
        /// <returns></returns>
        public static ValidationResult FailureResult(IActionResult errorResult, IDictionary<string, ModelBase> entityCache = null)
        {
            return new ValidationResult { Success = false, EntityCache = entityCache, ErrorResult = errorResult };
        }
    }
}
