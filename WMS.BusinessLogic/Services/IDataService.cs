using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Utils;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Services
{
    /// <summary>
    /// read and write
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public interface IDataService<T, TDto> : IReadService<T>
        where T : ModelBase
        where TDto : IHaveId
    {
        DataResult<T> Insert(TDto dto);
        DataResult<T> Update(TDto dto);
        DataResult<T> Delete(long id);
    }
}
