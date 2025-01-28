using FinalMvc.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Admin.Menu
{
    public class FoodCategoryVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public List<Food> Foods { get; set; }
    }
}
