using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ArtigosCientificosMvc.Service.Token
{
    public class TokenManager
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        
        public TokenManager(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }


        public async Task SetTokenAsync(string token)
        {
            await _protectedLocalStorage.SetAsync("auth_token", token);
        }

        
        public async Task<string> GetTokenAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("auth_token");
            return result.Success ? result.Value : null;
        }

        
        public async Task RemoveTokenAsync()
        {
            await _protectedLocalStorage.DeleteAsync("auth_token");
        }
    }
}
