using ArtigosCientificos.Api.Data;
using System.Security.Cryptography;
using ArtigosCientificos.Api.Services.JWTService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtigosCientificos.Api.Models.User;
using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.Role;

namespace ArtigosCientificos.Api.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly Jwt _jwt;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            this._jwt = new Jwt(configuration);
        }

        public async Task<ActionResult<User>> Register(UserDTO userDTO)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userDTO.Username))
            {
                return new BadRequestObjectResult("User already exists.");
            }


            var user = new User
            {
                Username = userDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                Role = _context.UserRoles.FirstOrDefaultAsync(r => r.Name == "Reviwer").Result
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }

        public async Task<(ActionResult<string>, UserToken)> Login(UserDTO userDTO)
        {
           
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username.Equals(userDTO.Username));

            if (!user.Username.Equals(userDTO.Username))
            {
                return (new BadRequestObjectResult("Invalid username or password."), null);
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return (new BadRequestObjectResult("Invalid username or password."), null);
            }

            var jwtToken = _jwt.CreateToken(user);

            var refreshToken = new UserToken
            {
                TokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,  
                Expired = DateTime.UtcNow.AddDays(7)
            };

            await SetRefreshToken(user, refreshToken);

            return (new OkObjectResult(jwtToken), refreshToken);
        }


        public async Task<(ActionResult<string>, UserToken)> RefreshToken(string currentRefreshToken)
        {
            var token = await _context.UserTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TokenValue == currentRefreshToken);

            if (token == null)
            {
                return (new BadRequestObjectResult("Invalid or expired refresh token."), null);
            }

            var user = token.User;

            if (user == null || token.Expired < DateTime.Now)
            {
                return (new BadRequestObjectResult("Invalid or expired refresh token."), null);
            }

            token.TokenValue = this._jwt.CreateToken(user);
            var newRefreshToken = this._jwt.GenerateRefreshToken();
            await SetRefreshToken(user, newRefreshToken);

            return (new OkObjectResult(token.TokenValue), newRefreshToken);
        }


        public async Task<UserToken> SetRefreshToken(User user, UserToken refreshToken)
        {
            refreshToken.UserId = user.Id;

            _context.UserTokens.Add(refreshToken);

            var oldTokens = _context.UserTokens.Where(t => t.UserId == user.Id && t.Expired < DateTime.Now);
            _context.UserTokens.RemoveRange(oldTokens);

            await _context.SaveChangesAsync();

            return refreshToken;
        }


    }
}
