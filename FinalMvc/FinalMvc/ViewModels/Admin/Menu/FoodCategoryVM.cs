using FinalMvc.Models;

namespace FinalMvc.ViewModels.Admin.Menu
{
    public class FoodCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Food> Foods { get; set; }
    }
}
