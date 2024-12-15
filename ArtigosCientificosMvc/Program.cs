using ArtigosCientificosMvc.Components;
using ArtigosCientificosMvc.Service;
using ArtigosCientificosMvc.Service.Api;
using ArtigosCientificosMvc.Service.Home;
using ArtigosCientificosMvc.Service.Login;
using ArtigosCientificosMvc.Service.Register;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddSingleton<ConfigServer>();
builder.Services.AddScoped<RequestHandler>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7267/api/");
})
.AddHttpMessageHandler<RequestHandler>()
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
