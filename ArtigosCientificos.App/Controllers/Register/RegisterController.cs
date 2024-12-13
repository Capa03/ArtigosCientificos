using ArtigosCientificos.App.Models.Register;
using ArtigosCientificos.App.Services.RegisterService;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.App.Controllers.Register
{
    public class RegisterController : Controller
    {

        private readonly IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            this._registerService = registerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDTO); 
            }

            string response = await this._registerService.Register(registerDTO);

            
            if (response == "User registered successfully.")
            {
                return RedirectToAction("Index", "Login"); 
            }
            else
            {
                ModelState.AddModelError(string.Empty, response); 
            }

            return View(registerDTO); 
        }


    }
}
