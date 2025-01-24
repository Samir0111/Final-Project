using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Admin.Chefsteams
{
    public class ChefsteamVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        public string Image { get; set; }

        public IFormFile Photo { get; set; }
    }
}
