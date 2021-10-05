using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WMS.DataAccess.Models;

namespace WMS.BusinessLogic.Utils
{
    /// <summary>
    /// Класс расширения DbContext
    /// </summary>
    public static class DbContextExtensions
    {
        public static IQueryable<T> GetAll<T>(this DbContext context, bool eager = false) where T : ModelBase
        {
            var query = context.Set<T>().AsQueryable();
            if (eager)
            {
                var navigations = context.Model.FindEntityType(typeof(T))
                    .GetDerivedTypesInclusive()
                    .SelectMany(type => type.GetNavigations())
                    .Distinct();

                foreach (var property in navigations)
                    query = query.Include(property.Name);
            }
            return query;
        }
    }
}
