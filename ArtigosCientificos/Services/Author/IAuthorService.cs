using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.Author
{
    /// <summary>
    /// Interface defining the contract for author-related services such as fetching user details.
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Retrieves a list of all users (authors) in the system.
        /// </summary>
        /// <returns>An <see cref="ObjectResult"/> containing a list of all users.</returns>
        Task<ObjectResult> GetAllUsers();

        /// <summary>
        /// Retrieves a specific user (author) by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be retrieved.</param>
        /// <returns>An <see cref="ObjectResult"/> containing the user details.</returns>
        Task<ObjectResult> GetUserById(int id);
    }
}
