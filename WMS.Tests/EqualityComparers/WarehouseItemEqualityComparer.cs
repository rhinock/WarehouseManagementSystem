using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WMS.DataAccess.Models;

namespace WMS.Tests.EqualityComparers
{
    /// <summary>
    /// Сравнение товаров на складе
    /// </summary>
    class WarehouseItemEqualityComparer : IEqualityComparer<WarehouseItem>
    {
        public bool Equals([AllowNull] WarehouseItem x, [AllowNull] WarehouseItem y)
        {
            if (x == null || y == null)
                return false;

            return
                x.Id == y.Id &&
                x.WarehouseId == y.WarehouseId &&
                x.ItemId == y.ItemId &&
                x.Count == y.Count;
        }

        public int GetHashCode([DisallowNull] WarehouseItem obj)
        {
            return base.GetHashCode();
        }
    }
}
