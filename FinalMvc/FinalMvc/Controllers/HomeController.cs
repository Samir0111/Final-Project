using FinalMvc.Data;
using FinalMvc.Models;
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

            var video = await _context.VideoPreviews
       .OrderByDescending(s => s.IsMain)
       .ToListAsync();


            var blogPosts = await _context.BlogPosts
              .OrderByDescending(b => b.Id) 
              .Take(3)
              .ToListAsync();
         var chefsTeams = await _context.Chefsteams.OrderByDescending(b => b.Id)
              .Take(3)
              .ToListAsync();

            var showCaseMenus = await _context.Showcasemenus.OrderByDescending(b => b.Id)
            .Take(8)
            .ToListAsync();

            var coreFeatures = await _context.CoreFeatures.OrderByDescending(b => b.Id)
           .Take(3)
           .ToListAsync();


           var appointmentSection = await _context.AppointmentSections.OrderByDescending(b => b.Id)
           .ToListAsync();

            var aboutSection = await _context.AboutSections.OrderByDescending(b => b.Id)
           .ToListAsync();


            return View(new HomeVM
            {


                Sliders = sliders,
               Showcasemenus = showCaseMenus,
                Chefsteams = chefsTeams,

                VideoPreviews = video,

                BlogPosts = blogPosts,

                CoreFeatures = coreFeatures,

                AppointmentSections = appointmentSection,

                AboutSections = aboutSection

            });
        }
    }
}
           