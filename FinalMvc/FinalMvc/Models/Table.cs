namespace FinalMvc.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }

        public int GuestCapacity { get; set; }

        public DateTime? AvailabilityStart { get; set; }
        public DateTime? AvailabilityEnd { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
                