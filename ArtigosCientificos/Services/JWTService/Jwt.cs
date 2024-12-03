using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ArtigosCientificos.Api.Services.JWTService
{
    public class Jwt
    {

        private readonly IConfiguration configuration;
        private readonly int EXPIRATION_TIME = 1;
        public Jwt(IConfiguration configuration)
        {

            this.configuration = configuration;
        }

        public string CreateToken(User user)
        {
            // Create a list of claims
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

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



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
