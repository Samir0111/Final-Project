using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalMvc.Services
{
    public class TestimonialService : ITestimonialService
    {
        private readonly AppDbContext _context;

        public TestimonialService(AppDbContext context)
        {
            _context = context;
        }

        // Save a new testimonial
        public async Task SaveTestimonial(Testimonial testimonial)
        {
            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();
        }

        // Update an existing testimonial
        public async Task UpdateTestimonialAsync(int id, Testimonial testimonial)
        {
            var existingTestimonial = await _context.Testimonials.FindAsync(id);
            if (existingTestimonial != null)
            {
                existingTestimonial.Comment = testimonial.Comment;
                existingTestimonial.Rating = testimonial.Rating;
                existingTestimonial.UserName = testimonial.UserName;
                existingTestimonial.IsPublished = testimonial.IsPublished;

                await _context.SaveChangesAsync();
            }
        }

        // Get a testimonial by ID
        public async Task<Testimonial> GetTestimonialByIdAsync(int id)
        {
            return await _context.Testimonials.FindAsync(id);
        }

        // Get all testimonials
        public async Task<List<Testimonial>> GetTestimonalAsync()
        {
            return await _context.Testimonials.ToListAsync();
        }

        // Delete a testimonial by ID
        public async Task DeleteTestimonialAsync(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
                await _context.SaveChangesAsync();
            }
        }
    }
}
