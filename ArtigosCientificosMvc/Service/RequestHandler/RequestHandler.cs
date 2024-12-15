using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

public class RequestHandler : DelegatingHandler
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;


    public RequestHandler(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
        
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Wait until JavaScript interop can be used, i.e., the component has rendered
        var token = await GetTokenFromLocalStorage();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetTokenFromLocalStorage()
    {
        try
        {
            // Defer token retrieval until after component render
            var token = await _protectedLocalStorage.GetAsync<string>("token");

            return token.Success ? token.Value : string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving token: " + ex.Message);
            return string.Empty;
        }
    }
}
