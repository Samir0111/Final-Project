using System;

namespace FinalMvc.ViewModels.Admin.Reservation
{
    public class ReservationVM
    {
        public int Id { get; set; }

        public int TableId { get; set; } // ✅ Table Identifier
        public int TableNumber { get; set; } // ✅ Table Number


        public DateTime Date { get; set; } // ✅ Reservation Date
        public DateTime ReserveTime { get; set; } // ✅ Reservation Start Time
        public DateTime EndReserveTime { get; set; } // ✅ Reservation End Time

        public string PhoneNumber { get; set; } // ✅ Contact Phone Number

        public string Status { get; set; } // ✅ Reservation Status (Pending, Approved, Declined)
    }
}
