using FinalMvc.Models;
using FinalMvc.Services.Interfaces;
using FinalMvc.ViewModels.Admin.Slider;

namespace FinalMvc.Services
{
    public class SliderService : ISliderService
    {
        public Task CreateAsync(Slider slider)
        {
            throw new NotImplementedException();
        }

        public Task<List<SliderVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SliderVM> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
