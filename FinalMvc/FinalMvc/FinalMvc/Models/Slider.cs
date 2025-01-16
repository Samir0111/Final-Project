using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public class Slider
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public bool IsMain { get; set; } = false;

        public string Desc { get; set; }

        public string SubTitle { get; set; }



        public string Image { get; set; }

        [NotMapped]
        [Required]
        public List<IFormFile> Photos { get; set; } 

    }
}
