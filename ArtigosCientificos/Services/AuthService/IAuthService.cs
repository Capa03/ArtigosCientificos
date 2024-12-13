using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<ActionResult<User>> Register(UserDTO userDTO);
        Task<(ActionResult<string>, UserToken)> Login(UserDTO userDTO);
        Task<(ActionResult<string>, UserToken)> RefreshToken();
        Task<UserToken> SetRefreshToken(User user, UserToken refreshToken);


        Task<ActionResult<List<User>>> GetAllUsers();
    }
}
