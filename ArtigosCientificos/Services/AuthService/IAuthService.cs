using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.AuthService
{
    public interface IAuthService
    {
        Task<ObjectResult> Register(UserDTO userDTO);
        Task<ObjectResult> Login(UserDTO userDTO);
        Task<ObjectResult> RefreshToken();
        Task<UserToken> SetRefreshToken(User user, UserToken refreshToken);

    }
}
