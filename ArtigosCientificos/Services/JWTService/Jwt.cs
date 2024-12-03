using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ArtigosCientificos.Api.Services.JWTService
{
    /// <summary>
    /// Service for creating and managing JWT and refresh tokens.
    /// </summary>
    public class Jwt
    {

        private readonly IConfiguration configuration;
        private readonly int EXPIRATION_TIME = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Jwt"/> class.
        /// </summary>
        /// <param name="configuration">Configuration object for accessing application settings.</param>
        public Jwt(IConfiguration configuration)
        {

            this.configuration = configuration;
        }


        /// <summary>
        /// Creates a JWT (JSON Web Token) for a specified user.
        /// </summary>
        /// <param name="user">The user object containing the user's details and roles.</param>
        /// <returns>A JWT string containing the user's claims and roles, signed with a secure key.</returns>
        /// <remarks>
        /// The generated token includes claims for the user's username and their roles. 
        /// The token is signed using HMAC SHA-512 and is valid for the configured expiration time (default: 1 hour).
        /// </remarks>
        public string CreateToken(User user)
        {
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            
            if (user.Role != null && user.Role.Any())
            {
                claims.AddRange(user.Role.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            }

            
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration.GetSection("AppSettings:Issuer").Value,
                audience: configuration.GetSection("AppSettings:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(EXPIRATION_TIME),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// <summary>
        /// Generates a secure refresh token for use in token renewal.
        /// </summary>
        /// <returns>A new <see cref="UserToken"/> object containing a secure random token value and expiration date.</returns>
        /// <remarks>
        /// - The refresh token is a random 64-byte base64-encoded string.
        /// - The token is set to expire in 24 hours from the time of generation.
        /// </remarks>
        public UserToken GenerateRefreshToken()
        {
            return new UserToken
            {
                TokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired = DateTime.Now.AddDays(1)
            };
        }

    }
}
