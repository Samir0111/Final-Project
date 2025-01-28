using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.VideoPreviews;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class VideoPreviewController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public VideoPreviewController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var videoItems = await _context.VideoPreviews.ToListAsync();
            var videoVMs = _mapper.Map<IEnumerable<VideoPreviewVM>>(videoItems);
            return View(videoVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var video = await _context.VideoPreviews.FirstOrDefaultAsync(v => v.Id == id);
            if (video == null) return NotFound();

            var model = _mapper.Map<VideoPreviewVM>(video);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoPreviewVM model)
        {
         
                var video = _mapper.Map<VideoPreview>(model);



            if (!await _context.VideoPreviews.AnyAsync())
            {
                video.IsMain = true;
            }
            else if (model.IsMain)
            {
                var otherVideos = await _context.VideoPreviews.Where(v => v.IsMain).ToListAsync();
                foreach (var vid in otherVideos) vid.IsMain = false;
            }

            if (model.Photo != null && !string.IsNullOrEmpty(model.YtLink))
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(stream);
                    }

                    video.PreviewImage = uniqueFileName;

                    if (model.IsMain)
                    {
                        var otherVideos = await _context.VideoPreviews.Where(v => v.IsMain).ToListAsync();
                        foreach (var vid in otherVideos) vid.IsMain = false;
                    }

                    await _context.VideoPreviews.AddAsync(video);
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
            VideoPreview video = await _context.VideoPreviews.FirstOrDefaultAsync(m => m.Id == id);
            if (video == null) return NotFound();

            video.IsMain = true;

            var currentVideo = await _context.VideoPreviews.FirstOrDefaultAsync(m => m.Id != id && m.IsMain);
            if (currentVideo != null)
            {
                currentVideo.IsMain = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var video = await _context.VideoPreviews.FirstOrDefaultAsync(v => v.Id == id);
            if (video == null) return NotFound();

            var model = _mapper.Map<VideoPreviewVM>(video);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VideoPreviewVM model)
        {
            if (id != model.Id) return BadRequest();

            var video = await _context.VideoPreviews.FindAsync(id);
            if (video == null) return NotFound();

            video.YtLink = model.YtLink;
            video.IsMain = model.IsMain;

            if (model.Photo != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                video.PreviewImage = uniqueFileName;
            }

            if (model.IsMain)
            {
                var otherVideos = await _context.VideoPreviews.Where(v => v.IsMain && v.Id != id).ToListAsync();
                foreach (var vid in otherVideos) vid.IsMain = false;
            }
            if (!string.IsNullOrEmpty(model.YtLink))
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
            var video = await _context.VideoPreviews.FindAsync(id);
            if (video == null) return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, "assets", "imgs", video.PreviewImage);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.VideoPreviews.Remove(video);
            await _context.SaveChangesAsync();

            var remainingVideos = await _context.VideoPreviews.ToListAsync();
            if (remainingVideos.Count == 1)
            {
                remainingVideos[0].IsMain = true;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
