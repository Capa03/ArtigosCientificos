using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ArtigosCientificosMvc.Models.User;
using ArtigosCientificosMvc.Service.Api;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ArtigosCientificosMvc.Service.Token
{
    /// <summary>
    /// Enum representing possible responses for token-related operations.
    /// </summary>
    public enum Response
    {
        NOT_FOUND_ID = 0,
        ERROR = -1
    }

    /// <summary>
    /// Manages authentication tokens, user data retrieval, and claim processing.
    /// </summary>
    public class TokenManager
    {
        private const string AuthTokenKey = "auth_token";
        private const string BaseClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";

        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly Lazy<ApiService> _apiService;
        private readonly ConfigServer _configServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenManager"/> class.
        /// </summary>
        /// <param name="protectedLocalStorage">The local storage service for storing tokens.</param>
        /// <param name="apiService">Lazy-initialized API service for making HTTP requests.</param>
        /// <param name="configServer">Configuration server containing API endpoint URLs.</param>
        public TokenManager(ProtectedLocalStorage protectedLocalStorage, Lazy<ApiService> apiService, ConfigServer configServer)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _apiService = apiService;
            _configServer = configServer;
        }

        /// <summary>
        /// Stores the authentication token in local storage.
        /// </summary>
        /// <param name="token">The token to store.</param>
        public async Task SetTokenAsync(string token)
        {
            await _protectedLocalStorage.SetAsync(AuthTokenKey, token);
        }

        /// <summary>
        /// Retrieves the authentication token from local storage.
        /// </summary>
        /// <returns>The stored token, or null if not found.</returns>
        public async Task<string?> GetTokenAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>(AuthTokenKey);
            
            return result.Success ? result.Value : null;
        }

        /// <summary>
        /// Removes the authentication token from local storage.
        /// </summary>
        public async Task RemoveTokenAsync() { 
            await _protectedLocalStorage.DeleteAsync(AuthTokenKey);
        }

        /// <summary>
        /// Checks if the user is authenticated by verifying token existence.
        /// </summary>
        /// <returns>True if the user has a token; otherwise, false.</returns>
        public async Task<bool> IsUserAuthenticated()
        {
            return !string.IsNullOrEmpty(await GetTokenAsync());
        }

        /// <summary>
        /// Retrieves the currently authenticated user's details.
        /// </summary>
        /// <returns>The authenticated user's details, or null if unavailable.</returns>
        public async Task<User?> GetUserAsync()
        {
            try
            {
                var userId = await GetUserIdAsync();
                if (userId <= 0)
                {
                    Console.WriteLine("Invalid userId. Returning null.");
                    return null;
                }

                var userUrl = $"{_configServer.GetUsersUrl()}{userId}";

                var user = await _apiService.Value.GetTAsync<User>(userUrl);
                if (user == null)
                {
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// Refreshes the authentication token by calling the API.
        /// </summary>
        /// <returns>The new token, or null if the refresh operation fails.</returns>
        public async Task<string?> RefreshTokenAsync()
        {
            return await _apiService.Value.GetTAsync<string>(_configServer.RefreshTokenUrl());
        }

        /// <summary>
        /// Retrieves the user ID from the authentication token.
        /// </summary>
        /// <returns>The user ID, or a response code indicating an error.</returns>
        public async Task<int> GetUserIdAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return (int)Response.ERROR;
            }

            var userIdClaim = ReadClaim(token, "sub") ?? ReadClaim(token, ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return (int)Response.NOT_FOUND_ID;
            }

            return int.TryParse(userIdClaim, out var userId) ? userId : (int)Response.ERROR;
        }


        /// <summary>
        /// Extracts a specific claim from the JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <param name="claimType">The type of claim to extract.</param>
        /// <returns>The value of the claim, or null if the claim is not found.</returns>
        /// <exception cref="ArgumentException">Thrown if the token is null or invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while reading the claim.</exception>
        private string? ReadClaim(string token, string claimType)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.ReadToken(token) is not JwtSecurityToken jwtToken)
                {
                    throw new ArgumentException("Invalid JWT token format.", nameof(token));
                }

                return jwtToken.Claims
                    .FirstOrDefault(claim => claim.Type == claimType || claim.Type == BaseClaim + claimType)
                    ?.Value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while reading the claim.", ex);
            }
        }
    }
}
