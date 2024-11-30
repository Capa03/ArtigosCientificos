using System.IdentityModel.Tokens.Jwt;

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
            return Ok(token);
        }



    }
}
