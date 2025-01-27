namespace FinalMvc.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; } 
        public decimal? SellPrice { get; set; } 
        public int FoodCategoryId { get; set; }
        public FoodCategory FoodCategory { get; set; }
    }

}
