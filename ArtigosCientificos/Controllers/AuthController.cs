using ArtigosCientificos.Api.Models.User;
using ArtigosCientificos.Api.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Controllers
{

    /// <summary>
    /// Handles authentication and user management operations such as login, registration, and token management.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service to handle user authentication logic.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userDTO">The data transfer object containing user registration details.</param>
        /// <returns>The created user object.</returns>
        /// <response code="200">Returns the newly registered user.</response>
        /// <response code="400">If the registration data is invalid.</response>

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDTO)
        {
            return await _authService.Register(userDTO);
        }

        /// <summary>
        /// Retrieves a list of all users. Requires authorization.
        /// </summary>
        /// <returns>A list of registered users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="401">If the user is not authorized.</response>
        /// 
        [HttpGet("users")]
        [Authorize(Roles = "Researcher")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _authService.GetAllUsers();
        }


        /// <summary>
        /// Authenticates a user and generates a refresh token.
        /// </summary>
        /// <param name="userDTO">The data transfer object containing user login credentials.</param>
        /// <returns>The authenticated user object.</returns>
        /// <response code="200">Returns the authenticated user and sets a refresh token cookie.</response>
        /// <response code="400">If the login credentials are invalid.</response>
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDTO userDTO)
        {
            var (result, refreshToken) = await _authService.Login(userDTO);

            if (refreshToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = refreshToken.Expired
                };
                Response.Cookies.Append("refreshToken", refreshToken.TokenValue, cookieOptions);
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Refreshes the authentication token using the stored refresh token.
        /// </summary>
        /// <returns>A new authentication token.</returns>
        /// <response code="200">Returns a new authentication token and updates the refresh token cookie.</response>
        /// <response code="400">If the refresh token is invalid or missing.</response>

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is empty or null.");
            }

            var (result, newRefreshToken) = await _authService.RefreshToken(refreshToken);

            if (newRefreshToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = newRefreshToken.Expired
                };
                Response.Cookies.Append("refreshToken", newRefreshToken.TokenValue, cookieOptions);
            }

            return Ok(result.Result);
        }

    }

}
