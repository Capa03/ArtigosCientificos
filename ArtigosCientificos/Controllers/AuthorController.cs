using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ArtigosCientificos.Api.Models.User;
using ArtigosCientificos.Api.Services.Author;

namespace ArtigosCientificos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {

        private readonly IAuthorService authorService;
        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        /// <summary>
        /// Retrieves a list of all users. Requires authorization.
        /// </summary>
        /// <returns>A list of registered users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="401">If the user is not authorized.</response>
        /// 
        [HttpGet("users")]
     
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {

            ObjectResult objectResult = await authorService.GetAllUsers();

            if (objectResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(objectResult.Value);
            }

            return Ok(objectResult.Value);
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            ObjectResult objectResult = await authorService.GetUserById(id);
            if (objectResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(objectResult.Value);
            }
            return Ok(objectResult.Value);
        }

    }
}
