using System.ComponentModel.DataAnnotations;

namespace FinalMvc.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int TableId { get; set; }

        public DateTime Date { get; set; }
        public DateTime ReserveTime { get; set; }
        public DateTime EndReserveTime { get; set; }

        public string UserId { get; set; }

        public Table Table { get; set; }

        public AppUser AppUser { get; set; }
       
        public string? Status { get; set; }

        public string? PhoneNumber { get; set; }


    }
}
