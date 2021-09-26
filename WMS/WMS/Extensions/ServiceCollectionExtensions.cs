using Microsoft.Extensions.DependencyInjection;
using WMS.DTO;
using WMS.Models;
using WMS.Services;

namespace WMS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрация сервиса как IReadService и IDataService
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="serviceCollection"></param>
        public static void RegisterDataService<TService, TEntity, TDto>(this IServiceCollection serviceCollection)
            where TService : class, IDataService<TEntity, TDto>
            where TEntity : ModelBase
            where TDto : IHaveId
        {
            serviceCollection.AddTransient<IReadService<TEntity>, TService>();
            serviceCollection.AddTransient<IDataService<TEntity, TDto>, TService>();
        }
    }
}
