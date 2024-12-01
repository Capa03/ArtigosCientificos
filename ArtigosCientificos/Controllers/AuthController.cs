using ArtigosCientificos.Api.Models.User;
using ArtigosCientificos.Api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDTO)
        {
            return await _authService.Register(userDTO);
        }

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


        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Invalid refresh token.");
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
