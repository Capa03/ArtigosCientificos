
using System.Text.Json;

namespace ArtigosCientificos.App.Services.ApiService
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseData);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception here
                Console.WriteLine($"Error making request: {ex.Message}");
                throw; // Rethrow or handle accordingly
            }
        }
    }

}
