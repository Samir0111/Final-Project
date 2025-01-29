using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using FinalMvc.ViewModels.Admin.Chefsteams;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ChefsteamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public ChefsteamController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var chefsItems = await _context.Chefsteams.ToListAsync();
            var chefsVMs = _mapper.Map<IEnumerable<ChefsteamVM>>(chefsItems);
            return View(chefsVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var chefs = await _context.Chefsteams.FirstOrDefaultAsync(n => n.Id == id);
            if (chefs == null) return NotFound();

            var model = _mapper.Map<ChefsteamVM>(chefs);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChefsteamVM model)
        {

            var chefs = _mapper.Map<Chefsteam>(model);

            if (model.Photo is not null & model.Name is not null & model.Position is not null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                chefs.Image = uniqueFileName;


                await _context.Chefsteams.AddAsync(chefs);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return (View(model));
            }

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var chefs = await _context.Chefsteams.FirstOrDefaultAsync(n => n.Id == id);
            if (chefs == null) return NotFound();

            var model = _mapper.Map<ChefsteamVM>(chefs);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChefsteamVM model)
        {
            if (id != model.Id) return BadRequest();

            var chefs = await _context.Chefsteams.FindAsync(id);
            if (chefs == null) return NotFound();

            chefs.Name = model.Name;
            chefs.Position = model.Position;

            if (model.Photo != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                chefs.Image = uniqueFileName;
            }
            if (model.Name is not null & model.Position is not null)
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
            var chefs = await _context.Chefsteams.FindAsync(id);
            if (chefs == null) return NotFound();



            _context.Chefsteams.Remove(chefs);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
