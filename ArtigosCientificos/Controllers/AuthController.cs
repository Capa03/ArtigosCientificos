using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using ArtigosCientificos.Api.JWTService;
using ArtigosCientificos.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ArtigosCientificos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration configuration;
        public AuthController(IConfiguration _configuration)
        {
            this.configuration = _configuration;    
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Invalid data");
            }

            user.Username = userDTO.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            user.Role = userDTO.Role ?? "Researcher"; 

            return Ok(user);
        }

        [HttpGet("researcher-data")]
        [Authorize(Roles = "Researcher")]
        public IActionResult GetResearcherData()
        {
            return Ok("This data is accessible only to Researchers.");
        }

        [HttpGet("reviewer-data")]
        [Authorize(Roles = "Reviewer")]
        public IActionResult GetReviewerData()
        {
            return Ok("This data is accessible only to Reviewers.");
        }



        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO userDTO)
        {

            if (user.Username != userDTO.Username)
            {
                return BadRequest("Invalid username");
            }
            if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
            {
                return BadRequest("Invalid password");
            }
            var token = new Jwt(this.configuration).CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Invalid refresh token");
            }
            if (user.RefreshToken != refreshToken)
            {
                return BadRequest("Invalid refresh token");
            }
            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                return BadRequest("Refresh token expired");
            }
            var token = new Jwt(this.configuration).CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);
            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken { 
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expired
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            user.RefreshToken = refreshToken.Token;
            user.CreationTime = refreshToken.Created;
            user.RefreshTokenExpiryTime = refreshToken.Expired;
        }

    }
}
