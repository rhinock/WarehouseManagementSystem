using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using WMS.DataAccess.Models;
using WMS.UI;

namespace WMS.Tests
{
    /// <summary>
    /// Тесты для склада
    /// </summary>
    public class WarehouseTests : IntegrationTestBase
    {
        public WarehouseTests(WebApplicationFactory<Startup> factory)
            : base(factory, "api/warehouses") { }
        /// <summary>
        /// Получить все склады
        /// </summary>
        /// <returns>True</returns>
        [Fact]
        public async Task GetAllWarehouses()
        {
            var response = await GetAsync();
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var warehouses = await DeserializeResponseAsync<List<Warehouse>>(response);
            Assert.NotNull(warehouses);
            Assert.True(warehouses.Count > 0);
        }
        /// <summary>
        /// Получить склад по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="maximumItems"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData(1, "miniature", 100)]
        [InlineData(2, "decent", 10000)]
        [InlineData(3, "hefty", 1000000)]
        public async Task GetWarehouseById(long id, string name, long maximumItems)
        {
            var response = await GetAsync(id);
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var warehouse = await DeserializeResponseAsync<Warehouse>(response);
            Assert.NotNull(warehouse);

            Assert.Equal(id, warehouse.Id);
            Assert.Equal(name, warehouse.Name);
            Assert.Equal(maximumItems, warehouse.MaximumItems);
        }
        /// <summary>
        /// Поиск склада по некорректном идентификатору (не найдено)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>StatusCodes.Status404NotFound</returns>
        [Theory]
        [InlineData(0), InlineData(long.MaxValue)]
        public async Task GetWarehouseByIncorrectId(long id)
        {
            var response = await GetAsync(id);
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
    }
}
