using System.Collections.Generic;
using WMS.BusinessLogic.Utils;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Services
{
    /// <summary>
    /// read-only
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadService<T> where T : ModelBase
    {
        DataResult<IEnumerable<T>> Get();
        DataResult<T> Get(long id);
    }
}
