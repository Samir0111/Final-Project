using Microsoft.AspNetCore.Mvc;

namespace FinalMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
