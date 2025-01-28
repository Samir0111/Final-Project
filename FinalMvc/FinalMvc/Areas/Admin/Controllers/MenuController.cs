using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using FinalMvc.ViewModels.Admin.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class MenuController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public MenuController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _context.Foods
                .Include(f => f.FoodCategory)
                .ToListAsync();
            return View(foods);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var food = await _context.Foods
                .Include(f => f.FoodCategory)
                .FirstOrDefaultAsync(f => f.Id == id);
            if (food == null) return NotFound();

            var model = _mapper.Map<MenuVM>(food);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.FoodCategories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuVM model)
        {

            if (model.SellPrice > model.Price || model.SellPrice <= 0)
            {
                ModelState.AddModelError("SellPrice", "Sell Price must be lower than the original Price or Null.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.FoodCategories.ToList();

            }

            var food = _mapper.Map<Food>(model);
           
            if (model.Photo != null
     && model.Price > 0
     && !string.IsNullOrWhiteSpace(model.Description)
     && model.FoodCategoryId > 0
     && !string.IsNullOrWhiteSpace(model.Name)
     &&  model.SellPrice < model.Price
      && model.SellPrice >0)
            { 
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }


                food.Image = uniqueFileName;
                await _context.Foods.AddAsync(food);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
         
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var food = await _context.Foods.FindAsync(id);
            if (food == null) return NotFound();

            var model = _mapper.Map<MenuVM>(food);
            ViewBag.Categories = _context.FoodCategories.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuVM model)
        {
            if (id != model.Id) return BadRequest();

            var food = await _context.Foods.FindAsync(id);
            if (food == null) return NotFound();

            if (model.SellPrice > model.Price)
            {
                ModelState.AddModelError("SellPrice", "Sell Price cannot be greater than the original Price.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.FoodCategories.ToListAsync();
            }

            if (model.Photo != null)
            {
                var existingImagePath = Path.Combine(_env.WebRootPath, "assets", "imgs", food.Image);
                if (System.IO.File.Exists(existingImagePath))
                {
                    System.IO.File.Delete(existingImagePath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                food.Image = uniqueFileName;
            }

            // Updating food properties
            food.Name = model.Name;
            food.Description = model.Description;
            food.Price = model.Price;
            food.SellPrice = model.SellPrice;
            food.FoodCategoryId = model.FoodCategoryId;

            if (model.SellPrice < model.Price
                
     && model.Price > 0
     && !string.IsNullOrWhiteSpace(model.Description)
     && model.FoodCategoryId > 0
     && !string.IsNullOrWhiteSpace(model.Name)
  
      && model.SellPrice > 0)
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
            var food = await _context.Foods.FindAsync(id);
            if (food == null) return NotFound();

            var imagePath = Path.Combine(_env.WebRootPath, "assets", "imgs", food.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
