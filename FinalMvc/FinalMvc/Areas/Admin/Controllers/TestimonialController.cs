using FinalMvc.Models;
using FinalMvc.Services;
using FinalMvc.Services.Interfaces;
using FinalMvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class TestimonialController : Controller
    {
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Retrieve testimonials and map to ViewModel
            var testimonials = await _testimonialService.GetTestimonalAsync();
            var viewModel = testimonials.Select(t => new TestimonialVM
            {
                Id = t.Id,
                UserName = t.UserName,
                Comment = t.Comment,
                Rating = t.Rating,
                IsPublished = t.IsPublished
            }).ToList();

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            // Fetch the testimonial by ID
            var testimonial = await _testimonialService.GetTestimonialByIdAsync(id);

            if (testimonial == null)
            {
                return NotFound(); // Return a 404 page if the testimonial is not found
            }

            // Map the testimonial to the ViewModel (if needed)
            var viewModel = new TestimonialVM
            {
                Id = testimonial.Id,
                UserName = testimonial.UserName,
                Comment = testimonial.Comment,
                Rating = testimonial.Rating,
                IsPublished = testimonial.IsPublished
            };

            return View(viewModel); // Pass the ViewModel to the Detail view
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var testimonial = await _testimonialService.GetTestimonialByIdAsync(id);
                if (testimonial == null)
                {
                    TempData["Error"] = "Testimonial not found.";
                    return RedirectToAction("Index");
                }

                testimonial.IsPublished = true; // Mark as published
                await _testimonialService.UpdateTestimonialAsync(id, testimonial);

                TempData["Message"] = "Testimonial approved successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _testimonialService.DeleteTestimonialAsync(id);
                TempData["Message"] = "Testimonial deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
