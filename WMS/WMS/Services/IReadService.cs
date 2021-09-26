using System.Collections.Generic;
using WMS.Models;
using WMS.Utils;

namespace WMS.Services
{
    /// <summary>
    /// сервис для данных (только чтение)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadService<T> where T : ModelBase
    {
        /// <summary>
        /// Получить данные
        /// </summary>
        /// <returns> DataResult<IEnumerable<T></returns>
        DataResult<IEnumerable<T>> Get();
        /// <summary>
        /// Получить данные по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DataResult<T></returns>
        DataResult<T> Get(long id);
    }
}
