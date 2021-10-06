namespace WMS.BusinessLogic.Utils
{
    public class DataResult<T> : BaseResult
    {
        public T Data { get; set; }
        public static DataResult<T> SuccessResult(BusinessResult businessResult, T data = default(T))
        {
            return new DataResult<T> { Success = true, BusinessResult = businessResult, Data = data };
        }
        public static DataResult<T> FailureResult(BusinessResult businessResult, string message = null, string url = null)
        {
            return new DataResult<T>() { BusinessResult = businessResult, Message = message, Url = url };
        }
    }
}
