using System.Text.Json;
using ArtigosCientificos.App.Models.Login;
using ArtigosCientificos.App.Models.User;
using ArtigosCientificos.App.Services.LoginService;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.App.Controllers.Login
{
    public class LoginController : Controller
    {

        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            this._loginService = loginService;
        }

        [HttpGet]

        public IActionResult Index()
        {
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
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Error");
            return View(userDto);
        }
    }
}
