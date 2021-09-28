using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.EqualityComparers;
using WMS;
using WMS.DTO;
using WMS.Models;
using Xunit;

namespace Tests
{
    /// <summary>
    /// Тесты товаров из складов
    /// </summary>
    public class WarehouseItemTests : IntegrationTestBase
    {
        public WarehouseItemTests(WebApplicationFactory<Startup> factory)
            : base(factory, "api/warehouseitems") { }
        
        /// <summary>
        /// Получить все товары из складов
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
        /// Получить товары из складов по идентификатору
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
        /// Создать товар на складе
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
        /// Создание товара на складе с использованием идентификатора (запрещено)
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
        /// Создать товар на складе с отрицательным количеством (запрещено)
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
        /// Изменить товар на складе
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
        /// Изменить товар на складе с использованием некорректного идентификатора (не найдено)
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
        /// Изменить товар на складе с использованием некорректных идентификаторов (не найдено)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>StatusCodes.Status404NotFound</returns>
        [Theory]
        [InlineData(1, -1, 1, 50)] // некорректный warehouseId
        [InlineData(2, 2, -2, 5000)] // некорректный itemId
        public async Task UpdateWarehouseItemWithIncorrectIds(long id, long warehouseId, long itemId, long count)
        {
            var response = await UpdateAsync(new WarehouseItemDto
                { Id = id, WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
        /// <summary>
        /// Изменить товар на складе с использованием некорректных данных (запрещено)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(3, 3, 3, -500000)] // некорретный count < 0
        [InlineData(1, 1, 1, 101)] // некорректный count > warehouse.MaximumItems
        public async Task UpdateWarehouseItemWithIncorrectData(long id, long warehouseId, long itemId, long count)
        {
            var response = await UpdateAsync(new WarehouseItemDto
            { Id = id, WarehouseId = warehouseId, ItemId = itemId, Count = count });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        /// Удалить товар со склада
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
        /// Удалить товар со склада с некорректным идентификатором (не найдено)
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
