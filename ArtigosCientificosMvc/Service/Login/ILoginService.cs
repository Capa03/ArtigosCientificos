using ArtigosCientificosMvc.Models.Login;

namespace ArtigosCientificosMvc.Service.Login
{
    /// <summary>
    /// Defines the methods related to user login functionality.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Asynchronously logs in a user using the provided login credentials.
        /// </summary>
        /// <param name="userDTO">The user credentials to authenticate. Contains the user's username and password.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result is a <see cref="LoginResult"/> object containing the login outcome.</returns>
        Task<LoginResult> Login(UserDTO userDTO);
    }
}
