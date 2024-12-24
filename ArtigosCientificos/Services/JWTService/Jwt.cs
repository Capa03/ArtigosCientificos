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
                new Claim(ClaimTypes.Name, user.Id.ToString())
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
        /// Generates a new refresh token with a random value and a 24-hour expiration.
        /// </summary>
        /// <returns>A newly created refresh token object.</returns>

        public UserToken GenerateRefreshToken()
        {
            return new UserToken
            {
                TokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddDays(1)
            };
        }

    }
}
