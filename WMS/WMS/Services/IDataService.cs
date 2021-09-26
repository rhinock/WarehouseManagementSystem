using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.DTO;
using WMS.Models;
using WMS.Utils;

namespace WMS.Services
{
    public interface IDataService<T, TDto> : IReadService<T>
        where T : ModelBase
        where TDto : IHaveId
    {
        DataResult<T> Insert(TDto dto);
        DataResult<T> Update(TDto dto);
        DataResult<T> Delete(long id);
    }
}
