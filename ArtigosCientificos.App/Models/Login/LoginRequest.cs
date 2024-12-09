namespace ArtigosCientificos.App.Models.Login
{
    public class LoginRequest
    {
        public string Value { get; set; } // Represents the JWT token string
        public List<object> Formatters { get; set; } // Formatters list, empty in the example
        public List<object> ContentTypes { get; set; } // Content types list, empty in the example
        public object DeclaredType { get; set; } // Can be null, so use object or nullable type
        public int? StatusCode { get; set; } // Status code of the response
    }
}
