using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using WMS.Models;

namespace WMS.Utils
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public IActionResult ErrorResult { get; set; }
        public IDictionary<string, ModelBase> EntityCache { get; set; }

        public static ValidationResult SuccessResult(IDictionary<string, ModelBase> entityCache = null)
        {
            return new ValidationResult { Success = true, EntityCache = entityCache, ErrorResult = null };
        }

        public static ValidationResult FailureResult(IActionResult errorResult, IDictionary<string, ModelBase> entityCache = null)
        {
            return new ValidationResult { Success = false, EntityCache = entityCache, ErrorResult = errorResult };
        }

        //public static implicit operator ValidationResult(ActionResult errorResult)
        //{
        //    return new ValidationResult { Success = false, ErrorResult = errorResult };
        //}
        //public static implicit operator ValidationResult(bool success)
        //{
        //    return new ValidationResult
        //    {
        //        Success = success,
        //        ErrorResult = success ? 
        //            new StatusCodeResult(StatusCodes.Status200OK) : 
        //            new StatusCodeResult(StatusCodes.Status500InternalServerError)
        //    };
        //}
    }
}
