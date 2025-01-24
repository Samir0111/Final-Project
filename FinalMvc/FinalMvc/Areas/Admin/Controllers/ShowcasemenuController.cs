using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.Showcasemenu;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ShowcasemenuController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public ShowcasemenuController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Showcasemenus.ToListAsync();
            return View(menus);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var menus = await _context.Showcasemenus.FirstOrDefaultAsync(b => b.Id == id);
            if (menus == null) return NotFound();
            var model = _mapper.Map<ShowcasemenuVM>(menus);

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowcasemenuVM model)
        {

            var menu = new Showcasemenu();

            if (model.Photo != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                menu.Foodpic = uniqueFileName;

                await _context.Showcasemenus.AddAsync(menu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var menu = await _context.Showcasemenus.FirstOrDefaultAsync(b => b.Id == id);
            if (menu == null) return NotFound();

            var model = _mapper.Map<ShowcasemenuVM>(menu);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShowcasemenuVM model)
        {
            if (id != model.Id) return BadRequest();

            var menu = await _context.Showcasemenus.FindAsync(id);
            if (menu == null) return NotFound();

            if (model.Photo != null)
            {
                var existingImagePath = Path.Combine(_env.WebRootPath, "assets", "imgs", menu.Foodpic);
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

                menu.Foodpic = uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var menu = await _context.Showcasemenus.FindAsync(id);
            if (menu == null) return NotFound();

            var imagePath = Path.Combine(_env.WebRootPath, "assets", "imgs", menu.Foodpic);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Showcasemenus.Remove(menu);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
