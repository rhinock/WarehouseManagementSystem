using System.Linq;

namespace WMS.Utils
{
    public class BaseResult
    {
        private BusinessResult[] businessResult = 
            new BusinessResult[] { BusinessResult.OK, BusinessResult.Created, BusinessResult.NoContent };
        // public bool Success => businessResult.Contains(BusinessResult);
        public bool Success { get; set; }
        // public BusinessResult BusinessResult { get; set; } = BusinessResult.OK;
        public BusinessResult BusinessResult { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
