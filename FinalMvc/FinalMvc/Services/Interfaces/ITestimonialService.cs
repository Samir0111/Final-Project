namespace FinalMvc.Services.Interfaces
{
    public interface ITestimonialService
    {
        Task SaveTestimonial(Testimonial testimonial);

        Task UpdateTestimonialAsync(int id, Testimonial testimonial);
        Task<Testimonial> GetTestimonialByIdAsync(int id);

        Task<List<Testimonial>> GetTestimonalAsync();

        Task DeleteTestimonialAsync(int id);

    }
}
