using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ArtigosCientificosMvc.Service.Token;
using Microsoft.AspNetCore.Components;

namespace ArtigosCientificosMvc.Service.Api
{
    public class ApiService
    {

        private readonly HttpClient _client;
        private readonly TokenManager _tokenManager;

        public ApiService(HttpClient client, TokenManager tokenManager)
        {
            _client = client;
            _tokenManager = tokenManager;
        }

        public async Task<T> GetTAsync<T>(string url)
        {
            var token = await _tokenManager.GetTokenAsync();
            try
            {
                // Set the Authorization header for the current request
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make the GET request
                var response = await _client.GetAsync(url);

                // Check if the token has expired based on the response headers
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var tokenExpired = response.Headers.Contains("Token-Expired");
                    if (tokenExpired)
                    {
                        Console.WriteLine("Token has expired.");
                        // Handle expired token here, e.g., prompt for login, refresh token, etc.
                        // Optionally, you can clear the token or take other action:
                        await _tokenManager.RemoveTokenAsync();
                        return default; // or you could return an error or initiate re-authentication
                    }
                }

                // If the response was successful, deserialize the content
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Request successful with status code: " + response.Content.ToString());
                    return JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    // Log failure if not successful
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    return default;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any exceptions that occur during the request
                Console.WriteLine("HttpRequestException: " + ex.Message);
                throw;
            }
        }



        public async Task<(T? Data, HttpStatusCode StatusCode)> PostAsync<T>(string url, object data)
        {

            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var token = await _tokenManager.GetTokenAsync();

            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
