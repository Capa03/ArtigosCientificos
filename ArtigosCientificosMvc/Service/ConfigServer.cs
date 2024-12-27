using ArtigosCientificosMvc.Components;

namespace ArtigosCientificosMvc.Service
{
    public class ConfigServer
    {

        string _baseUrl = "";

        public string GetLoginUrl()
        {
            return this._baseUrl + "Auth/login";
        }

        public string GetRegisterUrl()
        {
            return this._baseUrl + "Auth/register";
        }
        public string GetUsersUrl()
        {
            return this._baseUrl + "Author/users/";
        }

        public string GetArticlesCreateUrl()
        {
            return this._baseUrl + "Article/articles/";
        }

        public string GetReviewsUrl(string status)
        {
            return this._baseUrl + "Review/status/" + status;
        }

        public string GetReviewsByIdUrl(int id)
        {
            return this._baseUrl + "Review/" + id;
        }

        public string PUTReviewsByIdUrl(int id)
        {
            return this._baseUrl + "Review/" + id;
        }

        public string GetAcceptedArticlesUrl()
        {
            return this._baseUrl + "Article/articles/accepted";
        }
    }
}
