using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels
{
    public class TestimonialVM
    {
        public int Id { get; set; }

       
        public string UserName { get; set; } 

      
        public string Comment { get; set; } 

       
        public int Rating { get; set; }

        public bool IsPublished { get; set; } 
    }
}
