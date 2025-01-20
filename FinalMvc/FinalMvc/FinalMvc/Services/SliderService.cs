//using AutoMapper;
//using FinalMvc.Data;
//using FinalMvc.Models;
//using FinalMvc.Services.Interfaces;
//using FinalMvc.ViewModels.Admin.Slider;
//using Microsoft.EntityFrameworkCore;

//namespace FinalMvc.Services
//{
//    public class SliderService : ISliderService
//    {
//        private readonly AppDbContext _context;
//        private readonly IMapper _mapper;

//        public SliderService(AppDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task CreateAsync(Slider slider)
//        {
//            await _context.Sliders.AddAsync(slider);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<List<SliderVM>> GetAllAsync()
//        {
//            var sliders = await _context.Sliders.Include(s => s.Image).ToListAsync();
//            return _mapper.Map<List<SliderVM>>(sliders);
//        }

//        public async Task<SliderVM> GetByIdAsync(int id)
//        {
//            var slider = await _context.Sliders.Include(s => s.Image).FirstOrDefaultAsync(s => s.Id == id);
//            if (slider == null) throw new Exception("Slider not found");
//            return _mapper.Map<SliderVM>(slider);
//        }

//        public async Task UpdateAsync(SliderVM sliderVM)
//        {
//            var slider = await _context.Sliders.Include(s => s.Image).FirstOrDefaultAsync(s => s.Id == sliderVM.Id);
//            if (slider == null) throw new Exception("Slider not found");

//            slider.Title = sliderVM.Title;
//            slider.Desc = sliderVM.Desc;
//            slider.SubTitle = sliderVM.SubTitle;
//            slider.IsMain = sliderVM.IsMain;

//            // Handle image upload
//            if (sliderVM.Photo != null)
//            {
//                // Save new image and update path (pseudo-code: implement actual file handling logic)
//                slider.Image = "path/to/new/image";
//            }

//            _context.Sliders.Update(slider);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var slider = await _context.Sliders.Include(s => s.Image).FirstOrDefaultAsync(s => s.Id == id);
//            if (slider == null) throw new Exception("Slider not found");

//            _context.Sliders.Remove(slider);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
