using ArtigosCientificos.Api.Models.Token;
using ArtigosCientificos.Api.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.AuthService
{
    /// <summary>
    /// Interface defining the contract for authentication services such as login, registration, and token management.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userDTO">The user data transfer object containing the registration details.</param>
        /// <returns>An <see cref="ObjectResult"/> containing the result of the registration process.</returns>
        Task<ObjectResult> Register(UserDTO userDTO);

        /// <summary>
        /// Authenticates a user and generates an access token.
        /// </summary>
        /// <param name="userDTO">The user data transfer object containing the login credentials.</param>
        /// <returns>An <see cref="ObjectResult"/> containing the result of the login attempt, including the access token.</returns>
        Task<ObjectResult> Login(UserDTO userDTO);

        /// <summary>
        /// Refreshes the authentication token for a user.
        /// </summary>
        /// <returns>An <see cref="ObjectResult"/> containing the new token or error message.</returns>
        Task<ObjectResult> RefreshToken();

        /// <summary>
        /// Sets the refresh token for a user.
        /// </summary>
        /// <param name="user">The user for whom the refresh token will be set.</param>
        /// <param name="refreshToken">The refresh token to be set.</param>
        /// <returns>A <see cref="UserToken"/> object containing the refresh token.</returns>
        Task<UserToken> SetRefreshToken(User user, UserToken refreshToken);
    }
}
