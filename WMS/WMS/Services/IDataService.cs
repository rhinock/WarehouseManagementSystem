using WMS.DTO;
using WMS.Models;
using WMS.Utils;

namespace WMS.Services
{
    /// <summary>
    /// Интерфейс для сервиса данных (чтение и запись)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public interface IDataService<T, TDto> : IReadService<T>
        where T : ModelBase
        where TDto : IHaveId
    {
        /// <summary>
        /// Метод добавления
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>DataResult<T></returns>
        DataResult<T> Insert(TDto dto);
        /// <summary>
        /// Метод обновления
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>DataResult<T></returns>
        DataResult<T> Update(TDto dto);
        /// <summary>
        /// Метод удаления
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DataResult<T></returns>
        DataResult<T> Delete(long id);
    }
}
