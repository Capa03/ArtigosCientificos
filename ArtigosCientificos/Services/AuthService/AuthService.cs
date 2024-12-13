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

        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Token).ToListAsync();
        }

        /// <summary>
        /// Registers a new user with the role of 'Researcher'.
        /// </summary>
        /// <param name="userDTO">The data transfer object containing the user's registration details.</param>
        /// <returns>The newly created user object or an error if the username already exists.</returns>
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

        /// <summary>
        /// Logs in a user by verifying credentials and generating a JWT token and refresh token.
        /// </summary>
        /// <param name="userDTO">The data transfer object containing the user's login credentials.</param>
        /// <returns>A JWT token and a refresh token, or an error if the credentials are invalid.</returns>
        public async Task<ActionResult<string>> Login(UserDTO userDTO)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == userDTO.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
                return (new BadRequestObjectResult("Invalid username or password."));

            var jwtToken = _jwt.CreateToken(user);

            var refreshToken = _jwt.GenerateRefreshToken();
            await SetRefreshToken(user, refreshToken);

            return new OkObjectResult(jwtToken);
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
        public async Task<ActionResult<string>> RefreshToken()
        {
            
            var token = await _context.UserTokens
                .Include(t => t.User)
                .Include(t => t.User.Role)
                .Where(t => t.UserId == t.User.Id && t.Expired >= DateTime.UtcNow)  // Ensure token hasn't expired
                .OrderByDescending(t => t.Created)  // Order by creation date to get the most recent token
                .FirstOrDefaultAsync();  // Fetch the first token after sorting

            
            if (token == null || token.Expired <= DateTime.UtcNow)
            {
                return new BadRequestObjectResult("Invalid or expired refresh token.");
            }

            // Generate a new JWT and refresh token
            var newJwtToken = _jwt.CreateToken(token.User);
            var newRefreshToken = _jwt.GenerateRefreshToken();

            // Store the new refresh token (likely in a separate table or column)
            await SetRefreshToken(token.User, newRefreshToken);

            // Return the new JWT token as a string
            return new OkObjectResult(newJwtToken);  // Adjust this if you need to return both JWT and refresh token.
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
