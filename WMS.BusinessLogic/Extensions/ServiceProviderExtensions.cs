using System;

namespace WMS.BusinessLogic.Extensions
{
    /// <summary>
    /// Метод расширения для ServiceProvider
    /// </summary>
    public static class ServiceProviderExtensions
    {
        public static T Get<T>(this IServiceProvider serviceProvider) where T: class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
