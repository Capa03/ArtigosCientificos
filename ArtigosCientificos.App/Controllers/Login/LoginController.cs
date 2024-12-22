
using ArtigosCientificos.App.Models.Login;
using ArtigosCientificos.App.Models.User;
using ArtigosCientificos.App.Services.AuthService;
using ArtigosCientificos.App.Services.LoginService;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.App.Controllers.Login
{
    public class LoginController : Controller
    {

        private readonly ILoginService _loginService;
        private readonly IAuthService _authService;
        public LoginController(ILoginService loginService, IAuthService authService)
        {
            this._loginService = loginService;
            this._authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            this._authService.clearToken();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserDTO userDto)
        {

            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                LoginRequest? user = await _loginService.Login(userDto);
                if (user != null)
                {
                    if (user.StatusCode != 200)
                    {
                        ModelState.AddModelError("", "Invalid Credentials.");
                        return View(userDto);
                    }

                    Response.Cookies.Append("AuthToken", user.Value, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, 
                    });

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(userDto);
        }
    }
}
