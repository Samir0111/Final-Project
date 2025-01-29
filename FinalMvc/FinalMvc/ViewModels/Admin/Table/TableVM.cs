using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels
{
    public class TableVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Table Number")]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Guest capacity must be between 1 and 20.")]
        [Display(Name = "Guest Capacity")]
        public int GuestCapacity { get; set; }


        public DateTime? AvailabilityStart { get; set; }
        public DateTime? AvailabilityEnd { get; set; }



    }
}
