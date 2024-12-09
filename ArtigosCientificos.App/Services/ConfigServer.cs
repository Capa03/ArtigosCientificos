namespace ArtigosCientificos.App.Services
{
    public class ConfigServer
    {

        //private readonly string SERVER = "http://localhost:5262";
        //private readonly string DEFAULT_URL = "/api";

        public string GetLoginUrl()
        {
            return "https://localhost:7267/api/auth/login";
        }

    }
}
