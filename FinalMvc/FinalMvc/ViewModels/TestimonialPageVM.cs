using FinalMvc.Models;
using FinalMvc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.ViewModels
{
    public class TestimonialPageVM
    {

        public string Comment { get; set; }
        public int Rating { get; set; }
        public string UserName { get; set; }
        public string SuccessMessage { get; set; }


    }
}
