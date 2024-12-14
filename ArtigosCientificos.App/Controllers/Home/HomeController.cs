using ArtigosCientificos.App.Services.HomeService;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.App.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            this._homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await this._homeService.GetUsers();
            return View();
        }
    }
}
