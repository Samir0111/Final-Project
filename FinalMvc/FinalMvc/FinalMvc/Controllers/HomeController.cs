using FinalMvc.Data;
using FinalMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders
       .OrderByDescending(s => s.IsMain) 
       .ToListAsync();

            return View(new HomeVM
            {


                Sliders = sliders



            });
        }
    }
}
           