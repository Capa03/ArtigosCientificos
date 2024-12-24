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
    }
}
