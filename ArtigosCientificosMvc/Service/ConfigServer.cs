using ArtigosCientificosMvc.Components;

namespace ArtigosCientificosMvc.Service
{
    /// <summary>
    /// Provides methods to construct and return various API endpoint URLs for the application.
    /// </summary>
    public class ConfigServer
    {
        // Base URL of the API
        private string _baseUrl = "";

        /// <summary>
        /// Gets the URL for the login endpoint.
        /// </summary>
        /// <returns>The login URL as a string.</returns>
        public string GetLoginUrl()
        {
            return this._baseUrl + "Auth/login";
        }

        /// <summary>
        /// Gets the URL for the registration endpoint.
        /// </summary>
        /// <returns>The registration URL as a string.</returns>
        public string GetRegisterUrl()
        {
            return this._baseUrl + "Auth/register";
        }

        /// <summary>
        /// Gets the URL for the users endpoint.
        /// </summary>
        /// <returns>The users URL as a string.</returns>
        public string GetUsersUrl()
        {
            return this._baseUrl + "Author/users/";
        }

        /// <summary>
        /// Gets the URL for the refresh token endpoint.
        /// </summary>
        /// <returns>The refresh token URL as a string.</returns>
        public string RefreshTokenUrl()
        {
            return this._baseUrl + "Auth/refresh-token";
        }

        /// <summary>
        /// Gets the URL for creating articles.
        /// </summary>
        /// <returns>The articles creation URL as a string.</returns>
        public string GetArticlesCreateUrl()
        {
            return this._baseUrl + "Article/articles/";
        }

        /// <summary>
        /// Gets the URL for retrieving reviews by status.
        /// </summary>
        /// <param name="status">The status of the reviews (e.g., "Pending", "Approved").</param>
        /// <returns>The URL for fetching reviews by status.</returns>
        public string GetReviewsUrl(string status)
        {
            return this._baseUrl + "Review/status/" + status;
        }

        /// <summary>
        /// Gets the URL for retrieving a specific review by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>The URL for fetching a specific review by its ID.</returns>
        public string GetReviewsByIdUrl(int id)
        {
            return this._baseUrl + "Review/" + id;
        }

        /// <summary>
        /// Gets the URL for updating a specific review by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the review to be updated.</param>
        /// <returns>The URL for updating a specific review by its ID.</returns>
        public string PUTReviewsByIdUrl(int id)
        {
            return this._baseUrl + "Review/" + id;
        }

        /// <summary>
        /// Gets the URL for fetching accepted articles.
        /// </summary>
        /// <returns>The URL for fetching accepted articles.</returns>
        public string GetAcceptedArticlesUrl()
        {
            return this._baseUrl + "Article/articles/accepted";
        }
    }
}
