using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS;
using WMS.Models;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace Tests
{
    public class WarehouseTests : IntegrationTestBase
    {
        public WarehouseTests(WebApplicationFactory<Startup> factory)
            : base(factory, "api/warehouses") { }
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
        [Theory]
        [InlineData(1, "Склад миниатюрный", 100)]
        [InlineData(2, "Склад приличный", 10000)]
        [InlineData(3, "Склад здоровенный", 1000000)]
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
        [Theory]
        [InlineData(0), InlineData(long.MaxValue)]
        public async Task GetWarehouseByIncorrectId(long id)
        {
            var response = await GetAsync(id);
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
    }
}
