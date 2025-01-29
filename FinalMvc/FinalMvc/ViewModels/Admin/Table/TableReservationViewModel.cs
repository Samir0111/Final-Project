using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FinalMvc.ViewModels.Reservation
{
    public class TableReservationViewModel
    {
        public int TableId { get; set; }

        //public string UserId { get; set; }

        public int TableNumber { get; set; }

        [Required(ErrorMessage = "Please select a reservation date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please select a reservation start time.")]
        public DateTime ReserveTime { get; set; }

        public DateTime EndReserveTime { get; set; }

        [Required(ErrorMessage = "Please enter the number of guests.")]
        [Range(1, 20, ErrorMessage = "Guest count must be between 1 and 20.")]
        public int GuestCount { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+994(50|51|55|70|77|10)[1-9][0-9]{6}$", ErrorMessage = "Invalid phone number format. Example: +994514443024.")]
        public string PhoneNumber { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
