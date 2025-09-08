using ApiClient.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiClient.Application.Services
{
    public class HttpApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        public HttpApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T> GetDataAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Throws if not 2xx

            string content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return data;
        }
    }
}
