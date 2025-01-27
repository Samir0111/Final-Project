namespace FinalMvc.Models
{
    public class FoodCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Food> Foods { get; set; }
    }

}
