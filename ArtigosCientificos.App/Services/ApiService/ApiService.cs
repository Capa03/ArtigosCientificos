
using System.Net;
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


        public async Task<(T? Data, HttpStatusCode StatusCode)> PostAsync<T>(string url, object data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(url, content);

                var statusCode = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return (json, statusCode);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                }

                return (default, statusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("An error occurred while communicating with the server.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred.", ex);
            }
        }


    }

}
