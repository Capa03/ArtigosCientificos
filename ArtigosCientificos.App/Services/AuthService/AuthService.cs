using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ArtigosCientificos.App.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserAuthenticated()
        {
            var token = GetAuthToken();
            return !string.IsNullOrEmpty(token);
        }

        public string? GetAuthToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.Request.Cookies.TryGetValue("AuthToken", out var token) == true)
            {
                return token;
            }

            return null;
        }

        public string? GetUsername()
        {

            var token = GetAuthToken();

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

        public void clearToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Request.Cookies.TryGetValue("AuthToken", out var token) == true)
            {
                httpContext.Response.Cookies.Delete("AuthToken");

            }
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
