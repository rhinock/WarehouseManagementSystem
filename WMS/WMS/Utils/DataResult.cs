namespace WMS.Utils
{
    /// <summary>
    /// Кастомизированный результат обработки данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResult<T> : BaseResult
    {
        public T Data { get; set; }
        public static DataResult<T> SuccessResult(T data = default(T))
        {
            return new DataResult<T> { /*Success = true,*/ Data = data };
        }
        public static DataResult<T> FailureResult(BusinessResult errorResult, string message = null)
        {
            return new DataResult<T>() { ErrorResult = errorResult, Message = message };
        }
    }
}
