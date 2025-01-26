using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
          

            var video = await _context.VideoPreviews
       .OrderByDescending(s => s.IsMain)
       .ToListAsync();


            var chefsTeams = await _context.Chefsteams.OrderByDescending(b => b.Id)
                 .Take(3)
                 .ToListAsync();

          


            var appointmentSection = await _context.AppointmentSections.OrderByDescending(b => b.Id)
            .ToListAsync();

            var aboutSection = await _context.AboutSections.OrderByDescending(b => b.Id)
           .ToListAsync();


            return View(new AboutVM
            {


             
                Chefsteams = chefsTeams,

                VideoPreviews = video,

             

                AppointmentSections = appointmentSection,

                AboutSections = aboutSection

            });
        }
    }
}
