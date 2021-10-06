using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using WMS.UI;
using System.Threading.Tasks;

namespace WMS.Tests
{
    /// <summary>
    /// Базовые тесты
    /// </summary>
    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/Items")]
        [InlineData("/api/Warehouses")]
        [InlineData("/api/WarehouseItems")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
