using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.AboutSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AboutSectionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public AboutSectionController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var aboutItems = await _context.AboutSections.ToListAsync();
            var aboutVMs = _mapper.Map<IEnumerable<AboutSectionVM>>(aboutItems);
            return View(aboutVMs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutSectionVM model)
        {
            
                var about = _mapper.Map<AboutSection>(model);

            if (!await _context.AboutSections.AnyAsync())
            {
                about.IsMain = true;
            }
            else if (model.IsMain)
            {
                var otherItems = await _context.AboutSections.Where(v => v.IsMain).ToListAsync();
                foreach (var itemm in otherItems) itemm.IsMain = false;
            }

            if (model.ImageFile != null & model.Desc != null & model.PfpFile != null & model.Name != null & model.Title != null & model.Subtitle != null & model.Position != null)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    await model.PfpFile.CopyToAsync(stream);

                }

                about.Image = uniqueFileName;
                about.Pfp = uniqueFileName;


                await _context.AboutSections.AddAsync(about);
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

            var about = await _context.AboutSections.FirstOrDefaultAsync(n => n.Id == id);
            if (about == null) return NotFound();

            var model = _mapper.Map<AboutSectionVM>(about);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutSectionVM model)
        {
            if (id != model.Id) return BadRequest();

            var about = await _context.AboutSections.FindAsync(id);
            if (about == null) return NotFound();

            about.Title = model.Title;
            about.Subtitle = model.Subtitle;
            about.Desc = model.Desc;
            about.Name = model.Name;
            about.Position = model.Position;

            if (model.ImageFile != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                about.Image = uniqueFileName;
            }

            if (model.PfpFile != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.PfpFile.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PfpFile.CopyToAsync(stream);
                }

                about.Pfp = uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeMainStatus(int id)
        {
            AboutSection item = await _context.AboutSections.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null) return NotFound();

            item.IsMain = true;

            var currentItem = await _context.AboutSections.FirstOrDefaultAsync(m => m.Id != id && m.IsMain);
            if (currentItem != null)
            {
                currentItem.IsMain = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var about = await _context.AboutSections.FirstOrDefaultAsync(n => n.Id == id);
            if (about == null) return NotFound();

            var model = _mapper.Map<AboutSectionVM>(about);
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var about = await _context.AboutSections.FindAsync(id);
            if (about == null) return NotFound();

            _context.AboutSections.Remove(about);
            await _context.SaveChangesAsync();


            var remainingItems = await _context.AboutSections.ToListAsync();
            if (remainingItems.Count == 1)
            {
                remainingItems[0].IsMain = true;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
