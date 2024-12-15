using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace ArtigosCientificosMvc.Service.Api
{
    public class ApiService 
    {

        private readonly HttpClient _client;

        public ApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> GetTAsync<T>(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var tokenExpired = response.Headers.Contains("Token-Expired");
                    if (tokenExpired)
                    {
                        Console.WriteLine("Token has expired.");
                        // Handle expired token (e.g., prompt login or refresh token)
                        
                        return default;
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    return default;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HttpRequestException: " + ex.Message);
                throw;
            }
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
