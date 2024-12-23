using ArtigosCientificos.Api.Data;
using System.Security.Cryptography;
using ArtigosCientificos.Api.Services.JWTService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtigosCientificos.Api.Models.User;
using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.Role;
using System.Text.Json;


namespace ArtigosCientificos.Api.Services.AuthService
{
    /// <summary>
    /// Provides authentication and user management functionality, including registration, login, 
    /// token generation, and token refreshing.
    /// </summary>
    public enum Role
    {
        RESEARCHER,
        REVIEWER,
    }

    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly Jwt _jwt;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing user and token data.</param>
        /// <param name="configuration">The configuration object for JWT setup.</param>

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _jwt = new Jwt(configuration);
        }

        /// <summary>
        /// Retrieves all users from the database along with their roles and tokens.
        /// </summary>
        /// <returns>A list of users with their associated roles and tokens.</returns>

        public async Task<ObjectResult> GetAllUsers()
        {
            List<User> users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Token).ToListAsync();

            if (users == null)
            {
                return new NotFoundObjectResult("No users found.");
            }

            return new OkObjectResult(users);
        }

        /// <summary>
        /// Registers a new user with the role of 'Researcher'.
        /// </summary>
        /// <param name="userDTO">The data transfer object containing the user's registration details.</param>
        /// <returns>The newly created user object or an error if the username already exists.</returns>
        public async Task<ObjectResult> Register(UserDTO userDTO)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userDTO.Username))
                return new BadRequestObjectResult("User already exists.");

            var role = await _context.UserRoles.FirstOrDefaultAsync(role => role.Id == (int)Role.RESEARCHER + 1);
            if (role == null)
                return new NotFoundObjectResult("Role 'Researcher' does not exist.");

            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user.Role.Add(role);
            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }

        /// <summary>
        /// Logs in a user by verifying credentials and generating a JWT token and refresh token.
        /// </summary>
        /// <param name="userDTO">The data transfer object containing the user's login credentials.</param>
        /// <returns>A JWT token and a refresh token, or an error if the credentials are invalid.</returns>
        public async Task<ObjectResult> Login(UserDTO userDTO)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == userDTO.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
                return (new UnauthorizedObjectResult("Invalid username or password."));

            var jwtToken = _jwt.CreateToken(user);

            var refreshToken = _jwt.GenerateRefreshToken();
            await SetRefreshToken(user, refreshToken);

            return new OkObjectResult(JsonSerializer.Serialize(jwtToken));
        }

        /// <summary>
        /// Refreshes a JWT token using a valid refresh token.
        /// </summary>
        /// <param name="currentRefreshToken">The current refresh token provided by the client.</param>
        /// <returns>A new JWT token and a new refresh token, or an error if the provided refresh token is invalid or expired.</returns>
        /// <summary>
        /// Refreshes a JWT token using a valid refresh token.
        /// </summary>
        /// <param name="currentRefreshToken">The current refresh token provided by the client.</param>
        /// <returns>A new JWT token and a new refresh token, or an error if the provided refresh token is invalid or expired.</returns>
        public async Task<ObjectResult> RefreshToken()
        {

            var token = await _context.UserTokens
                .Include(t => t.User)
                .Include(t => t.User.Role)
                .Where(t => t.UserId == t.User.Id && t.Expired >= DateTime.UtcNow)
                .OrderByDescending(t => t.Created)
                .FirstOrDefaultAsync();


            if (token == null || token.Expired <= DateTime.UtcNow)
            {
                return new BadRequestObjectResult("Invalid or expired refresh token.");
            }

            var newJwtToken = _jwt.CreateToken(token.User);
            var newRefreshToken = _jwt.GenerateRefreshToken();

            await SetRefreshToken(token.User, newRefreshToken);

            return new OkObjectResult(newJwtToken);
        }




        /// <summary>
        /// Associates a refresh token with a user and removes expired tokens for that user.
        /// </summary>
        /// <param name="user">The user to associate the refresh token with.</param>
        /// <param name="refreshToken">The refresh token to set for the user.</param>
        /// <returns>The newly associated refresh token object.</returns>
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
