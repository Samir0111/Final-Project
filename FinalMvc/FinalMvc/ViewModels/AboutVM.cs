using FinalMvc.Models;
using FinalMvc.ViewModels;
using FinalMvc.ViewModels.Admin.AboutSection;
using FinalMvc.ViewModels.Admin.Slider;



namespace FinalMvc.ViewModels
{
    public class AboutVM
    {




        public List<Chefsteam> Chefsteams { get; set; }


        public List<VideoPreview> VideoPreviews { get; set; }



        public List<AppointmentSection> AppointmentSections { get; set; }

        public List<AboutSection> AboutSections { get; set; }


    }
}
