using AutoMapper;
using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class TableController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TableController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var tables = await _context.Tables.ToListAsync();
            var model = _mapper.Map<IEnumerable<TableVM>>(tables);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == id);
            if (table == null) return NotFound();
            var model = _mapper.Map<TableVM>(table);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TableVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var table = _mapper.Map<Table>(model);
            if (model.AvailabilityStart != null && model.AvailabilityEnd != null)
            {


                await _context.Tables.AddAsync(table);
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
            var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == id);
            if (table == null) return NotFound();
            var model = _mapper.Map<TableVM>(table);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TableVM model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var table = await _context.Tables.FindAsync(id);
            if (table == null) return NotFound();

            table.TableNumber = model.TableNumber;
            table.GuestCapacity = model.GuestCapacity;
            table.AvailabilityStart = model.AvailabilityStart;
            table.AvailabilityEnd = model.AvailabilityEnd;



            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null) return NotFound();

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
