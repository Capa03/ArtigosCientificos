
using System.Text;
using System.Text.Json;
using ArtigosCientificos.App.Models.Login;
using ArtigosCientificos.App.Models.User;

namespace ArtigosCientificos.App.Services.ApiService
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService(HttpClient httpClient)
        {
            _client = httpClient;
        }


        public async Task<T?> PostAsync<T>(string url, object data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response data: " + responseData);
                return JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error making request: {ex.Message}");
                throw;
            }
        }
    }

}
