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
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _loginService.Login(userDto);
                    if (user != null)
                    {
                        Console.WriteLine("Login success " + user.Value);
                        return RedirectToAction("Home");
                    }
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            return View(userDto);
        }

    }
}
