using Microsoft.Extensions.DependencyInjection;
using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Services;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register service as IReadService and IDataService
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
