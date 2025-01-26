using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var blogPosts = await _context.BlogPosts
             .OrderByDescending(b => b.Id)
             .Take(3)
             .ToListAsync();

            return View(new BlogVM
            {

                BlogPosts = blogPosts





            });


        }
    }
}
