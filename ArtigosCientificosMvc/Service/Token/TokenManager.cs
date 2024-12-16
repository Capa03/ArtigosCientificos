using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ArtigosCientificosMvc.Service.Token
{
    public class TokenManager
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        
        public TokenManager(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }


        public async Task SetTokenAsync(string token)
        {
            await _protectedLocalStorage.SetAsync("auth_token", token);
        }

        
        public async Task<string> GetTokenAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("auth_token");
            return result.Success ? result.Value : null;
        }

        
        public async Task RemoveTokenAsync()
        {
            await _protectedLocalStorage.DeleteAsync("auth_token");
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string> GetUsername()
        {

            var token = await GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                var username = ReadClaim(token, "name");
                if (string.IsNullOrWhiteSpace(username))
                {
                    return "Unknown User";
                }
                return username;
            }
            return null;
        }

        private string? ReadClaim(string token, string claimType)
        {
            string BASE_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            }

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    throw new ArgumentException("Invalid JWT token format.", nameof(token));
                }


                var claim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == claimType || claim.Type == BASE_CLAIM + claimType);

                return claim?.Value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while reading the claim.", ex);
            }
        }
    }
}
