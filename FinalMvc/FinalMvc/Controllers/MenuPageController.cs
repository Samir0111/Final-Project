using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Controllers
{
    public class MenuPageController : Controller
    {
        private readonly AppDbContext _context;

        public MenuPageController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category = null)
        {
            var foods = _context.Foods.Include(f => f.FoodCategory).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                foods = foods.Where(f => f.FoodCategory.Name == category);
            }

            var model = new MenuPageVM
            {
                Foods = await foods.ToListAsync(),
                Categories = await _context.FoodCategories.ToListAsync(),
                SelectedCategory = category
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var food = await _context.Foods
                .Include(f => f.FoodCategory)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (food == null) return NotFound();

            return View(food);
        }

    }
}
