using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using WMS;
using Xunit;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace Tests
{
    public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _url;
        public IntegrationTestBase(WebApplicationFactory<Startup> webApplicationFactory, string url)
        {
            _httpClient = webApplicationFactory.CreateClient();
            _url = url;
        }
        protected async Task<HttpResponseMessage> GetAsync()
        {
            return await _httpClient.GetAsync(_url);
        }
        protected async Task<HttpResponseMessage> GetAsync(long id)
        {
            return await _httpClient.GetAsync($"{_url}/{id}");
        }
        protected async Task<HttpResponseMessage> CreateAsync<T>(T dto)
        {
            var json = JsonSerializer.Serialize(dto);
            return await _httpClient.PostAsync(_url, new StringContent(json, Encoding.UTF8, "application/json"));
        }
        protected async Task<HttpResponseMessage> UpdateAsync<T>(T dto)
        {
            var json = JsonSerializer.Serialize(dto);
            return await _httpClient.PutAsync(_url, new StringContent(json, Encoding.UTF8, "application/json"));
        }
        protected async Task<HttpResponseMessage> DeleteAsync(long id)
        {
            return await _httpClient.DeleteAsync($"{_url}/{id}");
        }
        protected async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage httpResponseMessage)
            where T : class
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
