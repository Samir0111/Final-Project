using FinalMvc.Models;
using FinalMvc.ViewModels;
using FinalMvc.ViewModels.Admin.AboutSection;
using FinalMvc.ViewModels.Admin.Slider;



namespace FinalMvc.ViewModels
{
    public class HomeVM
    {

        public List<Slider> Sliders { get; set; }

        public List<Showcasemenu> Showcasemenus { get; set; }


        public List<Chefsteam> Chefsteams { get; set; }


        public List<VideoPreview> VideoPreviews { get; set; }

        public List<BlogPost> BlogPosts { get; set; }

        public List<CoreFeature> CoreFeatures { get; set; }

        public List<AppointmentSection> AppointmentSections { get; set; }

        public List<AboutSection> AboutSections { get; set; }


    }
}
