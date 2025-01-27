
using AutoMapper;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using FinalMvc.ViewModels.Admin.AboutSection;
using FinalMvc.ViewModels.Admin.AppointmentSection;
using FinalMvc.ViewModels.Admin.BlogPosts;
using FinalMvc.ViewModels.Admin.Chefsteams;
using FinalMvc.ViewModels.Admin.CoreFeatures;
using FinalMvc.ViewModels.Admin.Menu;
using FinalMvc.ViewModels.Admin.Showcasemenu;
using FinalMvc.ViewModels.Admin.Slider;
using FinalMvc.ViewModels.Admin.VideoPreviews;






namespace FinalMvc.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Slider, SliderVM>().ReverseMap(); ;
            CreateMap<Showcasemenu, ShowcasemenuVM>().ReverseMap();
            CreateMap<Chefsteam, ChefsteamVM>().ReverseMap();
            CreateMap<VideoPreview, VideoPreviewVM>().ReverseMap();
            CreateMap<BlogPost, BlogPostVM>().ReverseMap();
            CreateMap<CoreFeature, CoreFeatureVM>().ReverseMap();
            CreateMap<AboutSection, AboutSectionVM>().ReverseMap();
            CreateMap<AppointmentSection, AppointmentSectionVM>().ReverseMap();
            CreateMap<MenuVM, Food>()
                .ForMember(dest => dest.Image, opt => opt.Ignore()); // Ignore Photo property (handled manually)

            CreateMap<Food, MenuVM>();









        }
    }
}
