
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Admin.Slider
{
    public class SliderVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        public string SubTitle { get; set; }


        public bool IsMain { get; set; } = false;

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
