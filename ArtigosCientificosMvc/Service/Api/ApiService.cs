using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ArtigosCientificosMvc.Models.User;
using ArtigosCientificosMvc.Service.Token;
using Microsoft.Extensions.Logging;

namespace ArtigosCientificosMvc.Service.Api
{
    /// <summary>
    /// Service for making HTTP requests with token-based authentication and automatic token refresh.
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly TokenManager _tokenManager;
        private readonly ILogger<ApiService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiService"/> class.
        /// </summary>
        /// <param name="client">The HTTP client for sending requests.</param>
        /// <param name="tokenManager">The token manager for handling authentication tokens.</param>
        /// <param name="logger">Logger instance for logging activities and errors.</param>
        public ApiService(HttpClient client, TokenManager tokenManager, ILogger<ApiService> logger)
        {
            _client = client;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        /// <summary>
        /// Handles unauthorized responses and attempts to refresh the token if necessary.
        /// </summary>
        /// <param name="response">The HTTP response to check for authorization errors.</param>
        /// <returns>True if the token was successfully refreshed; otherwise, false.</returns>
        private async Task<bool> HandleUnauthorizedAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized && response.Headers.Contains("Token-Expired"))
            {
                _logger.LogWarning("Token expired. Attempting to refresh...");
                var refreshToken = await _tokenManager.RefreshTokenAsync();

                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogError("Token refresh failed. Re-authentication required.");
                    return false;
                }

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshToken);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses the HTTP response content into a specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="response">The HTTP response.</param>
        /// <returns>The deserialized response object, or default if the response indicates an error.</returns>
        private async Task<T?> ParseResponseAsync<T>(HttpResponseMessage response)
        {
            var responseData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogError($"Request failed. Status: {response.StatusCode}, Content: {responseData}");
            return default;
        }

        /// <summary>
        /// Sends a GET request to the specified URL and deserializes the response into the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>The deserialized response object, or null if the request fails.</returns>
        public async Task<T?> GetTAsync<T>(string url)
        {
            var token = await _tokenManager.GetTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _client.GetAsync(url);

                if (await HandleUnauthorizedAsync(response))
                {
                    response = await _client.GetAsync(url);
                }

                return await ParseResponseAsync<T>(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HttpRequestException: " + ex.Message, ex);
                throw new ApplicationException("An error occurred while making the GET request.", ex);
            }
        }

        /// <summary>
        /// Sends a POST request with the specified data to the given URL.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="data">The data to send in the POST request body.</param>
        /// <returns>A tuple containing the deserialized response object and the HTTP status code.</returns>
        public async Task<(T? Data, HttpStatusCode StatusCode)> PostAsync<T>(string url, object data)
        {
            return await SendRequestAsync<T>(url, data, HttpMethod.Post);
        }

        /// <summary>
        /// Sends a PUT request with the specified data to the given URL.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="data">The data to send in the PUT request body.</param>
        /// <returns>A tuple containing the deserialized response object and the HTTP status code.</returns>
        public async Task<(T? Data, HttpStatusCode StatusCode)> PutTAsync<T>(string url, object data)
        {
            return await SendRequestAsync<T>(url, data, HttpMethod.Put);
        }

        /// <summary>
        /// Sends an HTTP request with the specified method and data to the given URL.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="data">The data to send in the request body.</param>
        /// <param name="method">The HTTP method to use (POST or PUT).</param>
        /// <returns>A tuple containing the deserialized response object and the HTTP status code.</returns>
        /// <exception cref="NotSupportedException">Thrown if the HTTP method is not supported.</exception>
        public async Task<(T? Data, HttpStatusCode StatusCode)> SendRequestAsync<T>(string url, object data, HttpMethod method)
        {
            var token = await _tokenManager.GetTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response;

                if (method == HttpMethod.Post)
                    response = await _client.PostAsync(url, content);
                else if (method == HttpMethod.Put)
                    response = await _client.PutAsync(url, content);
                else
                    throw new NotSupportedException($"HTTP method '{method}' is not supported.");

                if (await HandleUnauthorizedAsync(response))
                {
                    if (method == HttpMethod.Post)
                        response = await _client.PostAsync(url, content);
                    else
                        response = await _client.PutAsync(url, content);
                }

                var dataResult = await ParseResponseAsync<T>(response);
                return (dataResult, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HttpRequestException during request." + ex);
                throw new ApplicationException("An error occurred while sending the request.", ex);
            }
        }
    }
}
