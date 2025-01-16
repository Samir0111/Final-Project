using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.Slider;

namespace FinalMvc.Services.Interfaces
{
    public interface ISliderService
    {
        Task CreateAsync(Slider slider);
        Task<List<SliderVM>> GetAllAsync();
        Task<SliderVM> GetByIdAsync(int id);
    }
}
