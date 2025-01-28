using FinalMvc.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Admin.Menu
{
    public class MenuVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? SellPrice { get; set; }

        public string Image { get; set; }

        public int FoodCategoryId { get; set; }

        public IFormFile Photo { get; set; } // For uploading files


    }
}
