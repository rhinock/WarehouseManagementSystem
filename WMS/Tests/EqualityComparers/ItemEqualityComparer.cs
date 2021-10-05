using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WMS.DataAccess.Models;

namespace WMS.Tests.EqualityComparers
{
    /// <summary>
    /// Сравнение двух товаров
    /// </summary>
    class ItemEqualityComparer : IEqualityComparer<Item>
    {
        public bool Equals([AllowNull] Item x, [AllowNull] Item y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Price == y.Price && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Item obj)
        {
            return base.GetHashCode();
        }
    }
}
