using System.Collections.Generic;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Utils
{
    public class ValidationResult : BaseResult
    {
        public IDictionary<string, ModelBase> EntityCache { get; set; }
        public static ValidationResult SuccessResult(BusinessResult businessResult, IDictionary<string, ModelBase> entityCache = null)
        {
            return new ValidationResult { Success = true, BusinessResult = businessResult, EntityCache = entityCache};
        }
        public static ValidationResult FailureResult(BusinessResult businessResult, string message = null, string url = null)
        {
            return new ValidationResult { BusinessResult = businessResult, Message = message, Url = url };

        }
    }
}
