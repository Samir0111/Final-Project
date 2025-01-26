using AutoMapper;
using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using FinalMvc.ViewModels.Admin.CoreFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TestimonialController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var testimonials = await _context.Testimonials.ToListAsync();
            var testimonialVMs = _mapper.Map<IEnumerable<TestimonialVM>>(testimonials);
            return View(testimonialVMs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var testimonial = await _context.Testimonials.FirstOrDefaultAsync(t => t.Id == id);
            if (testimonial == null) return NotFound();

            return View(testimonial);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null) return NotFound();

            testimonial.IsPublished = true;
            _context.Testimonials.Update(testimonial);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Testimonial approved successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null) return NotFound();

            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Testimonial deleted successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TestimonialVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model); // Return the form with validation messages
            }

            // Save testimonial to database
            var testimonial = new Testimonial
            {
                UserName = model.UserName,
                Comment = model.Comment,
                Rating = model.Rating,
                IsPublished = false // Not published by default
            };

            _context.Testimonials.Add(testimonial);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your testimonial has been submitted and is awaiting approval.";
            return RedirectToAction("Index"); // Redirect to the same page after submission
        }

    }
}
