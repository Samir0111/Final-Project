using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public class Chefsteam
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]

        public string Image { get; set; }
    }
}
