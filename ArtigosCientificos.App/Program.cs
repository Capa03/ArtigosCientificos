using ArtigosCientificos.Api.Services.TokenHandlerService;
using ArtigosCientificos.App.Services;
using ArtigosCientificos.App.Services.ApiService;
using ArtigosCientificos.App.Services.AuthService;
using ArtigosCientificos.App.Services.HomeService;
using ArtigosCientificos.App.Services.LoginService;
using ArtigosCientificos.App.Services.RegisterService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<ConfigServer>();
builder.Services.AddScoped<AuthTokenHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();; 


builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7267/api/");
})
.AddHttpMessageHandler<AuthTokenHandler>()
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
