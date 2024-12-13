using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.App.Controllers.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
