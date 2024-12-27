using ArtigosCientificosMvc.Components;
using ArtigosCientificosMvc.Service;
using ArtigosCientificosMvc.Service.Api;
using ArtigosCientificosMvc.Service.Articles;
using ArtigosCientificosMvc.Service.Home;
using ArtigosCientificosMvc.Service.Login;
using ArtigosCientificosMvc.Service.Register;
using ArtigosCientificosMvc.Service.Review;
using ArtigosCientificosMvc.Service.Token;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<Lazy<ApiService>>(sp => new Lazy<ApiService>(() => sp.GetRequiredService<ApiService>()));
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddSingleton<ConfigServer>();
builder.Services.AddScoped<TokenManager>();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7267/api/");
})
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
