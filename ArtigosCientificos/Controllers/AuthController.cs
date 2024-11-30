using System.IdentityModel.Tokens.Jwt;

using ArtigosCientificos.Api.JWTService;
using ArtigosCientificos.Api.Models;
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
            return Ok(user);
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
