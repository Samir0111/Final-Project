using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.Slider;
using AutoMapper;
using FinalMvc.Data;
using System.Reflection.Metadata.Ecma335;
using Org.BouncyCastle.Asn1.Ntt;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public SliderController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.ToListAsync();
            var sliderVMs = _mapper.Map<IEnumerable<SliderVM>>(sliders);
            return View(sliderVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();    

            var slider = await _context.Sliders.FirstOrDefaultAsync(t => t.Id == id);
            if (slider == null) return NotFound();

            var model = _mapper.Map<SliderVM>(slider);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderVM model)
        {


            var slider = _mapper.Map<Slider>(model);


            if (!await _context.Sliders.AnyAsync())
            {
                slider.IsMain = true; 
            }
            else if (model.IsMain)
            {
                var otherSldiers = await _context.Sliders.Where(v => v.IsMain).ToListAsync();
                foreach (var sli in otherSldiers) sli.IsMain = false;
            }

            if (model.Photo is not null & model.SubTitle is not null & model.Title is not null & model.Desc is not null)
            {

                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                slider.Image = uniqueFileName;


                await _context.Sliders.AddAsync(slider);
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

            var slider = await _context.Sliders.FirstOrDefaultAsync(t => t.Id == id);
            if (slider == null) return NotFound();

            var model = _mapper.Map<SliderVM>(slider);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderVM model)
        {
            if (id != model.Id) return BadRequest();

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            slider.Title = model.Title;
            slider.SubTitle = model.SubTitle;
            slider.Desc = model.Desc;

            slider.IsMain = model.IsMain;



            if (model.Photo != null)
            {


                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var newFilePath = Path.Combine(_env.WebRootPath, "assets", "imgs", uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                slider.Image = uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();



            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            var sliderss = await _context.Sliders.ToListAsync();
            if (sliderss.Count == 1)
            {
                sliderss[0].IsMain = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeMainStatus(int id)
        {
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null) return NotFound();

            slider.IsMain = true;

            var currentMainSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id != id && m.IsMain);
            if (currentMainSlider != null)
            {
                currentMainSlider.IsMain = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
