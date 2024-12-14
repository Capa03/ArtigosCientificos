namespace ArtigosCientificos.App.Services
{
    public class ConfigServer
    {

        public string GetLoginUrl()
        {
            return "Auth/login";
        }

        public string GetRegisterUrl()
        {
            return "Auth/register";
        }

        public string GetUsersUrl()
        {
            return "Auth/users";
        }
    }
}
