using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WMS.Models;

namespace Tests.EqualityComparers
{
    class WarehouseEqualityComparer : IEqualityComparer<Warehouse>
    {
        public bool Equals([AllowNull] Warehouse x, [AllowNull] Warehouse y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.MaximumItems == y.MaximumItems && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Warehouse obj)
        {
            return base.GetHashCode();
        }
    }
}
