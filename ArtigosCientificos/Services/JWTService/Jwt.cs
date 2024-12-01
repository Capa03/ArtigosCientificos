using ArtigosCientificos.Api.Models;
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
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

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

        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired = DateTime.Now.AddDays(7)
            };
        }

    }
}
