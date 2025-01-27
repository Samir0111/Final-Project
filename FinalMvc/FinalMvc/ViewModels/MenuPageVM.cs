using FinalMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalMvc.ViewModels
{
    public class MenuPageVM
    {
        public List<FoodCategory> Categories { get; set; }
        public List<Food> Foods { get; set; }
        public string SelectedCategory { get; set; }
    }
}
