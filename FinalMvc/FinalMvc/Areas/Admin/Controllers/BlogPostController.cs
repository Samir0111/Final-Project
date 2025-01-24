using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FinalMvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.BlogPosts;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class BlogPostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public BlogPostController(AppDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await _context.BlogPosts.ToListAsync();
            var blogPostVMs = _mapper.Map<IEnumerable<BlogPostVM>>(blogPosts);
            return View(blogPostVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(n => n.Id == id);
            if (blogPost == null) return NotFound();

            var model = _mapper.Map<BlogPostVM>(blogPost);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostVM model)
        {
            
                var blogPost = _mapper.Map<BlogPost>(model);

                if (model.Photo is not null & model.ByWho is not null & model.Date is not null & model.Category is not null & model.Description is not null & model.Title is not null)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(stream);
                    }

                    blogPost.Image = uniqueFileName;

                await _context.BlogPosts.AddAsync(blogPost);
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

            var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(n => n.Id == id);
            if (blogPost == null) return NotFound();

            var model = _mapper.Map<BlogPostVM>(blogPost);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPostVM model)
        {
            if (id != model.Id) return BadRequest();

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();

            blogPost.Category = model.Category;
            blogPost.Date = model.Date;
            blogPost.ByWho = model.ByWho;
            blogPost.Title = model.Title;
            blogPost.Description = model.Description;

            if (model.Photo != null)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs");
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                blogPost.Image = uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, "assets", "imgs", blogPost.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
