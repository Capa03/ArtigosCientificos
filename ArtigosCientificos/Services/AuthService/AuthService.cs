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

    public enum Role
    {
        RESEARCHER,
        REVIEWER,
    }

    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly Jwt _jwt;


        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _jwt = new Jwt(configuration);
        }

        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Token).ToListAsync();
        }


        public async Task<ActionResult<User>> Register(UserDTO userDTO)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userDTO.Username))
                return new BadRequestObjectResult("User already exists.");

            var role = await _context.UserRoles.FirstOrDefaultAsync(role => role.Id == (int)Role.RESEARCHER + 1);
            if (role == null)
                return new BadRequestObjectResult("Role 'Researcher' does not exist.");

            var user = new User
            {
                Username = userDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user.Role.Add(role);
            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }

        public async Task<(ActionResult<string>, UserToken)> Login(UserDTO userDTO)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == userDTO.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
                return (new BadRequestObjectResult("Invalid username or password."), null);

            var jwtToken = _jwt.CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            await SetRefreshToken(user, refreshToken);

            return (new OkObjectResult(jwtToken), refreshToken);
        }

        public async Task<(ActionResult<string>, UserToken)> RefreshToken(string currentRefreshToken)
        {
          
            var token = await _context.UserTokens
                .Include(t => t.User)
                .Include(t => t.User.Role)
                .FirstOrDefaultAsync(t => t.TokenValue == currentRefreshToken);

            if (token == null || token.Expired <= DateTime.UtcNow)
                return (new BadRequestObjectResult("Invalid or expired refresh token."), null);

            var newJwtToken = _jwt.CreateToken(token.User);
            var newRefreshToken = GenerateRefreshToken();

            await SetRefreshToken(token.User, newRefreshToken);

            return (new OkObjectResult(newJwtToken), newRefreshToken);
        }


        private UserToken GenerateRefreshToken()
        {
            return new UserToken
            {
                TokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,
                Expired = DateTime.UtcNow.AddDays(1)
            };
        }

        public async Task<UserToken> SetRefreshToken(User user, UserToken refreshToken)
        {
            refreshToken.UserId = user.Id;

            _context.UserTokens.Add(refreshToken);

            var expiredTokens = _context.UserTokens
                .Where(t => t.UserId == user.Id && t.Expired <= DateTime.UtcNow);
            _context.UserTokens.RemoveRange(expiredTokens);

            await _context.SaveChangesAsync();

            return refreshToken;
        }
    }
}
