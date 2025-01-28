using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.AppointmentSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AppointmentSectionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentSectionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var sections = await _context.AppointmentSections.ToListAsync();
            var model = _mapper.Map<IEnumerable<AppointmentSectionVM>>(sections);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var section = await _context.AppointmentSections.FirstOrDefaultAsync(s => s.Id == id);
            if (section == null) return NotFound();
            var model = _mapper.Map<AppointmentSectionVM>(section);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentSectionVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var section = _mapper.Map<AppointmentSection>(model);

            if (!await _context.AppointmentSections.AnyAsync())
            {
                section.IsMain = true;
            }
            else if (model.IsMain)
            {
                var otherItems = await _context.AboutSections.Where(v => v.IsMain).ToListAsync();
                foreach (var itemm in otherItems) itemm.IsMain = false;
            }

            if (model.Catch is not null & model.Subtitle is not null & model.Title is not null)
            {
                await _context.AppointmentSections.AddAsync(section);
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
        public async Task<IActionResult> ChangeMainStatus(int id)
        {
            AppointmentSection item = await _context.AppointmentSections.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null) return NotFound();

            item.IsMain = true;

            var currentItem = await _context.AppointmentSections.FirstOrDefaultAsync(m => m.Id != id && m.IsMain);
            if (currentItem != null)
            {
                currentItem.IsMain = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var section = await _context.AppointmentSections.FirstOrDefaultAsync(s => s.Id == id);
            if (section == null) return NotFound();
            var model = _mapper.Map<AppointmentSectionVM>(section);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentSectionVM model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var section = await _context.AppointmentSections.FindAsync(id);
            if (section == null) return NotFound();

            section.Catch = model.Catch;
            section.Title = model.Title;
            section.Subtitle = model.Subtitle;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var section = await _context.AppointmentSections.FindAsync(id);
            if (section == null) return NotFound();

            _context.AppointmentSections.Remove(section);
            await _context.SaveChangesAsync();

            var remainingItems = await _context.AppointmentSections.ToListAsync();
            if (remainingItems.Count == 1)
            {
                remainingItems[0].IsMain = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
