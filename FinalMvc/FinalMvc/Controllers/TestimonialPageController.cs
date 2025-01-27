using FinalMvc.Models;
using FinalMvc.Services.Interfaces;
using FinalMvc.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinalMvc.Controllers
{
    public class TestimonialPageController : Controller
    {
        private readonly ITestimonialService _testimonialService;
        private readonly IValidator<TestimonialPageVM> _testimonialvalidator;


        public TestimonialPageController(ITestimonialService testimonialService, IValidator<TestimonialPageVM> testimonialvalidator)
        {
            _testimonialService = testimonialService;
            _testimonialvalidator = testimonialvalidator;

        }
        [HttpGet]
        public IActionResult AddTestimonial()
        {
            return View(new TestimonialPageVM());
        }

        [HttpPost]
        public async Task<ActionResult> AddTestimonial(TestimonialPageVM viewModel)
        {

            var validation = await _testimonialvalidator.ValidateAsync(viewModel);

            if (!validation.IsValid)
            {

                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(viewModel);
            }


            var testimonial = new Testimonial
            {
                Comment = viewModel.Comment,
                UserName = viewModel.UserName,
                Rating = viewModel.Rating,
            };


            await _testimonialService.SaveTestimonial(testimonial);


            viewModel.SuccessMessage = "Your message has been sent successfully!";
            ModelState.Clear();


            return View(new TestimonialPageVM { SuccessMessage = viewModel.SuccessMessage });
        }
    }
}
