using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.EqualityComparers;
using WMS;
using WMS.DTO;
using WMS.Extensions;
using WMS.Models;
using Xunit;

namespace Tests
{
    public class ItemTests : IntegrationTestBase
    {
        public ItemTests(WebApplicationFactory<Startup> factory)
            : base(factory, "api/items") { }
        /// <summary>
        /// Получить все товары
        /// </summary>
        /// <returns>True</returns>
        [Fact]
        public async Task GetAllItems()
        {
            var response = await GetAsync();
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var items = await DeserializeResponseAsync<List<Item>>(response);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
        }
        /// <summary>
        /// Получить товар по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData(1, "Карандаш", 10.00)]
        [InlineData(2, "Ручка", 20.00)]
        [InlineData(3, "Фломастер", 30.00)]
        public async Task GetItemById(long id, string name, decimal price)
        {
            var response = await GetAsync(id);
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var item = await DeserializeResponseAsync<Item>(response);
            Assert.NotNull(item);

            Assert.Equal(id, item.Id);
            Assert.Equal(name, item.Name);
            Assert.Equal(price, item.Price);
        }
        /// <summary>
        /// Создать товар
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData("Мелок", 15.00)]
        [InlineData("Гуашь", 22.00)]
        [InlineData("Акрил", 322.00)]
        public async Task CreateItem(string name, decimal price)
        {
            var response = await CreateAsync(new ItemDto { Name = name, Price = price });
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var item = await DeserializeResponseAsync<Item>(response);
            Assert.NotNull(item);
            Assert.True(item.Id > 0);
            Assert.NotNull(item.Name);

            response = await GetAsync(item.Id);
            response.EnsureSuccessStatusCode();

            var itemCheck = await DeserializeResponseAsync<Item>(response);
            Assert.NotNull(itemCheck);

            ItemEqualityComparer itemEqualityComparer = new ItemEqualityComparer();
            Assert.True(itemEqualityComparer.Equals(item, itemCheck));
        }
        /// <summary>
        /// Создать товар с идентификатором (запрещено)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(-1, "Мелок", 15.00)]
        [InlineData(22, "Гуашь", 22.00)]
        [InlineData(4, "Акрил", 322.00)]
        public async Task CreateItemWithId(long id, string name, decimal price)
        {
            var response = await CreateAsync(new ItemDto { Id = id, Name = name, Price = price });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        /// Создать товар с некорректной ценой (запрещено)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>Status400BadRequest</returns>
        [Theory]
        [InlineData("Мелок", -15.00)]
        [InlineData("Гуашь", -22.00)]
        [InlineData("Акрил", -1.00)]
        public async Task CreateItemWithNegativePrice(string name, decimal price)
        {
            var response = await CreateAsync(new ItemDto { Name = name, Price = price });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        /// Изменить товар
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("Мелок", 15.00)]
        [InlineData("Гуашь", 22.00)]
        [InlineData("Акрил", 322.00)]
        public async Task UpdateItem(string name, decimal price)
        {
            var response = await CreateAsync(new ItemDto { Name = name.Reverse(), Price = price * 10 });
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            var itemCreated = await DeserializeResponseAsync<Item>(response);
            Assert.NotNull(itemCreated);
            Assert.True(itemCreated.Id > 0);

            response = await UpdateAsync(new ItemDto { Id = itemCreated.Id, Name = name, Price = price });
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();

            response = await GetAsync(itemCreated.Id);
            response.EnsureSuccessStatusCode();

            var itemUpdated = await DeserializeResponseAsync<Item>(response);
            Assert.NotNull(itemUpdated);

            Assert.Equal(itemCreated.Id, itemUpdated.Id);
            Assert.Equal(name, itemUpdated.Name);
            Assert.Equal(price, itemUpdated.Price);
        }
        /// <summary>
        /// Изменение товара с некорректным идентификатором (не найдено)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>StatusCodes.Status404NotFound</returns>
        [Theory]
        [InlineData(0, "Мелок", 15.00)] // непередача Id
        [InlineData(-5, "Гуашь", 22.00)] // передача несуществующего Id
        [InlineData(long.MaxValue, "Акрил", 322.00)] // передача несуществующего Id
        public async Task UpdateItemWithIncorrectId(long id, string name, decimal price)
        {
            var response = await UpdateAsync(new ItemDto { Id = id, Name = name, Price = price });
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
        /// <summary>
        /// Изменение товара с некорректными данными (запрещено)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(1, null, 10.00)]
        [InlineData(2, "\t", 20.00)]
        [InlineData(3, "", 30.00)]
        [InlineData(1, "Карандаш", -10.00)]
        public async Task UpdateItemWithIncorrectData(long id, string name, decimal price)
        {
            var response = await UpdateAsync(new ItemDto { Id = id, Name = name, Price = price });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
    }
}
