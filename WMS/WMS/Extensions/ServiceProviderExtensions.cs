using System;

namespace WMS.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T Get<T>(this IServiceProvider serviceProvider) where T: class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
