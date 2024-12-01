using ArtigosCientificos.Api.Data;
using System.Security.Cryptography;
using ArtigosCientificos.Api.Models;
using ArtigosCientificos.Api.Services.JWTService;
using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtigosCientificos.Api.Models.User;

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
                Role = "Researcher"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }

        public async Task<(ActionResult<User>, RefreshToken)> Login(UserDTO userDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
            {
                return (new BadRequestObjectResult("Invalid username or password."), null);
            }

            var token = this._jwt.CreateToken(user);
            var refreshToken = this._jwt.GenerateRefreshToken();
            await SetRefreshToken(user, refreshToken);

            return (new OkObjectResult(token), refreshToken);
        }


        public async Task<(ActionResult<string>, RefreshToken)> RefreshToken(string currentRefreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == currentRefreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.Now)
            {
                return (new BadRequestObjectResult("Invalid or expired refresh token."), null);
            }

            var token = this._jwt.CreateToken(user);
            var newRefreshToken = this._jwt.GenerateRefreshToken();
            await SetRefreshToken(user, newRefreshToken);

            return (new OkObjectResult(token), newRefreshToken);
        }


        public async Task<RefreshToken> SetRefreshToken(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken.Token;
            user.CreationTime = refreshToken.Created;
            user.RefreshTokenExpiryTime = refreshToken.Expired;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return refreshToken; 
        }

    }
}
