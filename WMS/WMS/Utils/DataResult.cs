using Microsoft.AspNetCore.Mvc;


namespace WMS.Utils
{
    public class DataResult<T>
    {
        public bool Success { get; set; }
        public IActionResult ErrorResult { get; set; }
        public T Data { get; set; }
        public static DataResult<T> SuccessResult(T data = default(T))
        {
            return new DataResult<T> { Success = true, Data = data };
        }
        public static DataResult<T> FailureResult(IActionResult errorResult)
        {
            return new DataResult<T> { Success = false, ErrorResult = errorResult };
        }
    }
}
