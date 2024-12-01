using ArtigosCientificos.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArtigosCientificos.Api.JWTService
{
    public class Jwt
    {

        private readonly IConfiguration configuration;
        private readonly int EXPIRATION_TIME = 1;
        public Jwt(IConfiguration configuration) {

            this.configuration = configuration;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role) 
        };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetSection("AppSettings:Token").Value!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: this.configuration.GetSection("AppSettings:Issuer").Value,
                audience: this.configuration.GetSection("AppSettings:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(this.EXPIRATION_TIME),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
