using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Tests.EqualityComparers;
using WMS.UI;
using WMS.BusinessLogic.DTO;
using WMS.DataAccess.Models;
using Xunit;

namespace WMS.Tests
{
    public class WarehouseItemTests : IntegrationTestBase
    {
        public WarehouseItemTests(WebApplicationFactory<Startup> factory)
            : base(factory, "api/warehouseitems") { }
        
        /// <summary>
        /// Get all WarehouseItems
        /// </summary>
        /// <returns>True</returns>
        [Fact]
        public async Task GetAllWarehouseItems()
        {
            var response = await GetAsync();
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var items = await DeserializeResponseAsync<List<WarehouseItem>>(response);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
        }
        /// <summary>
        /// Get WarehouseItem by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData(1, 1, 1, 50)]
        [InlineData(2, 2, 2, 5000)]
        [InlineData(3, 3, 3, 500000)]
        public async Task GetWarehouseItemById(long id, long warehouseId, long itemId, long count)
        {
            var response = await GetAsync(id);
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var warehouseItem = await DeserializeResponseAsync<WarehouseItem>(response);
            Assert.NotNull(warehouseItem);

            Assert.Equal(id, warehouseItem.Id);
            Assert.Equal(warehouseId, warehouseItem.WarehouseId);
            Assert.Equal(itemId, warehouseItem.ItemId);
            Assert.Equal(count, warehouseItem.Count);
        }
        /// <summary>
        /// Create a WarehouseItem
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData(1, 2, 50)]
        [InlineData(2, 3, 5000)]
        [InlineData(3, 1, 500000)]
        public async Task CreateWarehouseItem(long warehouseId, long itemId, long count)
        {
            var response = await CreateAsync(new WarehouseItemDto { WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var warehouseItem = await DeserializeResponseAsync<WarehouseItem>(response);
            Assert.NotNull(warehouseItem);
            Assert.True(warehouseItem.Id > 0);

            try
            {
                response = await GetAsync(warehouseItem.Id);
                response.EnsureSuccessStatusCode();

                var warehouseItemCheck = await DeserializeResponseAsync<WarehouseItem>(response);
                Assert.NotNull(warehouseItemCheck);

                WarehouseItemEqualityComparer warehouseItemEqualityComparer = new WarehouseItemEqualityComparer();
                Assert.True(warehouseItemEqualityComparer.Equals(warehouseItem, warehouseItemCheck));
            }
            finally
            {
                await DeleteAsync(warehouseItem.Id);
            }
        }
        /// <summary>
        /// Create a WarehouseItem with Id (prohibited)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(1, 1, 1, 50)]
        [InlineData(2, 2, 2, 5000)]
        [InlineData(3, 3, 3, 500000)]
        public async Task CreateWarehouseItemWithId(long id, long warehouseId, long itemId, long count)
        {
            var response = await CreateAsync(new WarehouseItemDto { Id = id, WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        ///  Create a WarehouseItem with negative Count (prohibited)
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(1, 1, -50)]
        [InlineData(2, 2, -5000)]
        [InlineData(3, 3, -500000)]
        public async Task CreateWarehouseItemWithNegativeCount(long warehouseId, long itemId, long count)
        {
            var response = await CreateAsync(new WarehouseItemDto { WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        /// Edit WarehouseItem
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData(1, 2, 50)]
        [InlineData(2, 3, 5000)]
        [InlineData(3, 1, 500000)]
        public async Task UpdateWarehouseItem(long warehouseId, long itemId, long count)
        {
            var response = await CreateAsync(new WarehouseItemDto 
                { WarehouseId = warehouseId, ItemId = itemId, Count = count / 5 });
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var warehouseItemCreated = await DeserializeResponseAsync<WarehouseItem>(response);
            Assert.NotNull(warehouseItemCreated);
            Assert.True(warehouseItemCreated.Id > 0);

            try
            {
                response = await UpdateAsync(new WarehouseItemDto
                    { Id = warehouseItemCreated.Id, WarehouseId = warehouseId, ItemId = itemId, Count = count });
                Assert.NotNull(response);
                response.EnsureSuccessStatusCode();

                response = await GetAsync(warehouseItemCreated.Id);
                response.EnsureSuccessStatusCode();

                var warehouseItemUpdated = await DeserializeResponseAsync<WarehouseItem>(response);
                Assert.NotNull(warehouseItemUpdated);

                Assert.Equal(warehouseItemCreated.Id, warehouseItemUpdated.Id);
                Assert.Equal(warehouseId, warehouseItemUpdated.WarehouseId);
                Assert.Equal(itemId, warehouseItemUpdated.ItemId);
                Assert.Equal(count, warehouseItemUpdated.Count);
            }
            finally
            {
                await DeleteAsync(warehouseItemCreated.Id);
            }
        }
        /// <summary>
        /// Edit WarehouseItem with incorrect Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>StatusCodes.Status404NotFound</returns>
        [Theory]
        [InlineData(0, 1, 1, 50)] // непередача Id
        [InlineData(-5, 2, 2, 5000)] // передача несуществующего Id
        [InlineData(long.MaxValue, 3, 3, 500000)] // передача несуществующего Id
        public async Task UpdateWarehouseItemWithIncorrectId(long id, long warehouseId, long itemId, long count)
        {
            var response = await UpdateAsync(new WarehouseItemDto
                { Id = id, WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
        /// <summary>
        /// Edit WarehouseItem with incorrect WarehouseId or ItemId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>StatusCodes.Status404NotFound</returns>
        [Theory]
        [InlineData(1, -1, 1, 50)] // WarehouseId
        [InlineData(2, 2, -2, 5000)] // ItemId
        public async Task UpdateWarehouseItemWithIncorrectIds(long id, long warehouseId, long itemId, long count)
        {
            var response = await UpdateAsync(new WarehouseItemDto
                { Id = id, WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
        /// <summary>
        /// Edit WarehouseId with incorrect data (prohibited)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(3, 3, 3, -500000)] // count < 0
        [InlineData(1, 1, 1, 101)] // count > warehouse.MaximumItems
        public async Task UpdateWarehouseItemWithIncorrectData(long id, long warehouseId, long itemId, long count)
        {
            var response = await UpdateAsync(new WarehouseItemDto
            { Id = id, WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        /// Delete WarehouseItem
        /// </summary>
        /// <returns>True</returns>
        [Fact]
        public async Task DeleteWarehouseItem()
        {
            var response = await CreateAsync(new WarehouseItemDto { WarehouseId = 1, ItemId = 2, Count = 5 });
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var warehouseItemCreated = await DeserializeResponseAsync<WarehouseItem>(response);
            Assert.NotNull(warehouseItemCreated);
            Assert.True(warehouseItemCreated.Id > 0);

            response = await DeleteAsync(warehouseItemCreated.Id);
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);

            response = await GetAsync(warehouseItemCreated.Id);
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
        /// <summary>
        /// Delete WarehouseItem with incorrect Id
        /// </summary>
        /// <returns>StatusCodes.Status404NotFound</returns>
        [Fact]
        public async Task DeleteWarehouseItemWithIncorrectId()
        {
            var response = await DeleteAsync(long.MaxValue);
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
    }
}
