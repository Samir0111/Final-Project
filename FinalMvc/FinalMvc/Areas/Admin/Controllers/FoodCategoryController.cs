using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class FoodCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public FoodCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.FoodCategories
                .Include(c => c.Foods) 
                .ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var category = await _context.FoodCategories
                .Include(c => c.Foods) // Include related foods
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodCategoryVM model)
        {
        

            if (model.Name is not null)
            {

                var category = new FoodCategory
                {
                    Name = model.Name
                };

                await _context.FoodCategories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                return View(model);

            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.FoodCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new FoodCategoryVM
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodCategoryVM model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
           


            var category = await _context.FoodCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            if (model.Name is not null)
            {

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.FoodCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.FoodCategories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Category deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
