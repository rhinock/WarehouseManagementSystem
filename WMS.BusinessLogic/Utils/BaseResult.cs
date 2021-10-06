namespace WMS.BusinessLogic.Utils
{
    /// <summary>
    /// Базовый результат обработки данных
    /// </summary>
    public class BaseResult
    {
        public bool Success { get; set; }
        public BusinessResult BusinessResult { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
