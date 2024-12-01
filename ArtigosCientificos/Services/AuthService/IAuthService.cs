using ArtigosCientificos.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<ActionResult<User>> Register(UserDTO userDTO);
        Task<(ActionResult<User>, RefreshToken)> Login(UserDTO userDTO);
        Task<(ActionResult<string>, RefreshToken)> RefreshToken(string currentRefreshToken);
        RefreshToken GenerateRefreshToken();
        Task<RefreshToken> SetRefreshToken(User user, RefreshToken refreshToken);
    }
}
