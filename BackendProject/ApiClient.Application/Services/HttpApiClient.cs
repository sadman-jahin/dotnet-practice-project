using ApiClient.Application.Interfaces;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Text;
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
                .WaitAndRetryAsync(2, attempt => TimeSpan.FromSeconds(Math.Pow(60, attempt)));
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

        public async Task<T> PostDataAsync<T>(string url, object data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.PostAsync(url, content));
            response.EnsureSuccessStatusCode(); // Throws if not 2xx

            string responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public async Task<T> PutDataAsync<T>(string url, object data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.PutAsync(url, content));
            response.EnsureSuccessStatusCode(); // Throws if not 2xx

            string responseContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseContent))
            {
                return default;
            }

            try
            {
                var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            catch (Exception ex)
            {
                return (T)(object)responseContent;
            }
        }

        public async Task<bool> DeleteDataAsync(string url)
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.DeleteAsync(url));
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
