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

        // Index - List all categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.FoodCategories
                .Include(c => c.Foods) // Include related foods
                .ToListAsync();
            return View(categories);
        }

        // Detail - View details of a specific category and its foods
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

        // Create - Display form
        public IActionResult Create()
        {
            return View();
        }

        // Create - Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodCategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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

        // Edit - Display form
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

        // Edit - Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodCategoryVM model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = await _context.FoodCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Category updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Delete
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
