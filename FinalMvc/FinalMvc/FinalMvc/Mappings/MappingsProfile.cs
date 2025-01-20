
using AutoMapper;
using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.Slider;






namespace FinalMvc.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Slider, SliderVM>().ReverseMap(); ;

        }
    }
}
