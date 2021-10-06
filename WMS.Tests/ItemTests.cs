using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Tests.EqualityComparers;
using WMS.UI;
using WMS.BusinessLogic.DTO;
using WMS.BusinessLogic.Extensions;
using WMS.DataAccess.Models;
using Xunit;

namespace WMS.Tests
{
    public class ItemTests : IntegrationTestBase
    {
        public ItemTests(WebApplicationFactory<Startup> factory)
            : base(factory, "api/items") { }

        /// <summary>
        /// Get all Items
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
        /// Get Item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData(1, "pencil", 10.00)]
        [InlineData(2, "pen", 20.00)]
        [InlineData(3, "felt-tip pen", 30.00)]
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
        /// Create an Item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData("crayon", 15.00)]
        [InlineData("gouache", 22.00)]
        [InlineData("acrylic", 322.00)]
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
        /// Create an Item with Id (prohibited)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(-1, "crayon", 15.00)]
        [InlineData(22, "gouache", 22.00)]
        [InlineData(4, "acrylic", 322.00)]
        public async Task CreateItemWithId(long id, string name, decimal price)
        {
            var response = await CreateAsync(new ItemDto { Id = id, Name = name, Price = price });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        /// Create an item with incorrect price (prohibited)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>Status400BadRequest</returns>
        [Theory]
        [InlineData("crayon", -15.00)]
        [InlineData("gouache", -22.00)]
        [InlineData("acrylic", -1.00)]
        public async Task CreateItemWithNegativePrice(string name, decimal price)
        {
            var response = await CreateAsync(new ItemDto { Name = name, Price = price });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
        /// <summary>
        /// Edit an item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>True</returns>
        [Theory]
        [InlineData("crayon2", 15.00)]
        [InlineData("gouache2", 22.00)]
        [InlineData("acrylic2", 322.00)]
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
        /// Edit an item with incorrect Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>StatusCodes.Status404NotFound</returns>
        [Theory]
        [InlineData(0, "crayon", 15.00)] // непередача Id
        [InlineData(-5, "gouache", 22.00)] // передача несуществующего Id
        [InlineData(long.MaxValue, "acrylic", 322.00)] // передача несуществующего Id
        public async Task UpdateItemWithIncorrectId(long id, string name, decimal price)
        {
            var response = await UpdateAsync(new ItemDto { Id = id, Name = name, Price = price });
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
        /// <summary>
        /// Edit an item with incorrect data (prohibited)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns>StatusCodes.Status400BadRequest</returns>
        [Theory]
        [InlineData(1, null, 10.00)]
        [InlineData(2, "\t", 20.00)]
        [InlineData(3, "", 30.00)]
        [InlineData(1, "pencil", -10.00)]
        public async Task UpdateItemWithIncorrectData(long id, string name, decimal price)
        {
            var response = await UpdateAsync(new ItemDto { Id = id, Name = name, Price = price });
            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }
    }
}
