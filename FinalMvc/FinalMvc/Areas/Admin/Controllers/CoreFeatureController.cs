using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.CoreFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CoreFeatureController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public CoreFeatureController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var features = await _context.CoreFeatures.ToListAsync();
            var featureVMs = _mapper.Map<IEnumerable<CoreFeatureVM>>(features);
            return View(featureVMs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoreFeatureVM model)
        {
          
                var feature = _mapper.Map<CoreFeature>(model);

                if (model.Photo != null & model.Title != null & model.Description != null) 
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(stream);
                    }

                    feature.Image = uniqueFileName;

                await _context.CoreFeatures.AddAsync(feature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            else
            {
                return View(model);

            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var feature = await _context.CoreFeatures.FirstOrDefaultAsync(n => n.Id == id);
            if (feature == null) return NotFound();

            var model = _mapper.Map<CoreFeatureVM>(feature);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoreFeatureVM model)
        {
            if (id != model.Id) return BadRequest();

            var feature = await _context.CoreFeatures.FindAsync(id);
            if (feature == null) return NotFound();

            feature.Title = model.Title;
            feature.Description = model.Description;

            if (model.Photo != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                feature.Image = uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var feature = await _context.CoreFeatures.FirstOrDefaultAsync(n => n.Id == id);
            if (feature == null) return NotFound();

            var model = _mapper.Map<CoreFeatureVM>(feature);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _context.CoreFeatures.FindAsync(id);
            if (feature == null) return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, "assets", "imgs", feature.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.CoreFeatures.Remove(feature);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
