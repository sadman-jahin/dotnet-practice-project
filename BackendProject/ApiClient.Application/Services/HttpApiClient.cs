using ApiClient.Application.Interfaces;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiClient.Application.Services
{
    public class HttpApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;

        public HttpApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        }

        public async Task<T> GetDataAsync<T>(string url)
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));

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
