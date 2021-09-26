using System.Collections.Generic;
using WMS.Models;
using WMS.Utils;

namespace WMS.Services
{
    public interface IReadService<T> where T : ModelBase
    {
        DataResult<IEnumerable<T>> Get();
        DataResult<T> Get(long id);
    }
}
