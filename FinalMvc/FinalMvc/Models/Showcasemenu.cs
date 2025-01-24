using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public class Showcasemenu
    {

        public int Id { get; set; }

        [Required]
        public string Foodpic { get; set; }
    }
}
