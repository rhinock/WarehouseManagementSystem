using System.Linq;

namespace WMS.Utils
{
    public class BaseResult
    {
        private BusinessResult[] errorResults = new BusinessResult[] { BusinessResult.OK, BusinessResult.Created, BusinessResult.NoContent };
        public bool Success => errorResults.Contains(ErrorResult);
        public BusinessResult ErrorResult { get; set; }
        public string Message { get; set; }
    }
}
