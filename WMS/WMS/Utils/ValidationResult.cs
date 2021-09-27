using System.Collections.Generic;
using WMS.Models;

namespace WMS.Utils
{
    /// <summary>
    /// Класс для валидации результата
    /// </summary>
    public class ValidationResult : BaseResult
    {
        /// <summary>
        /// Кэш для сущностей
        /// </summary>
        public IDictionary<string, ModelBase> EntityCache { get; set; }
        public static ValidationResult SuccessResult(IDictionary<string, ModelBase> entityCache = null)
        {
            return new ValidationResult { /*Success = true,*/ EntityCache = entityCache};
        }
        public static ValidationResult FailureResult(BusinessResult errorResult, string message = null)
        {
            return new ValidationResult { ErrorResult = errorResult, Message = message };

        }
    }
}
