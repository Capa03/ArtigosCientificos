using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Models.Register;

namespace ArtigosCientificosMvc.Service.Register
{
    /// <summary>
    /// Defines the methods related to user registration functionality.
    /// </summary>
    public interface IRegisterService
    {
        /// <summary>
        /// Asynchronously registers a new user using the provided registration details.
        /// </summary>
        /// <param name="registerDTO">The registration details of the user, which includes the user's username, password, and other relevant information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result is a <see cref="RegisterResult"/> object containing the result of the registration attempt.</returns>
        Task<RegisterResult> Register(UserDTO registerDTO);
    }
}
