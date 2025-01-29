using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Reservation
{
    public class AvailableTimeSlotsViewModel
    {
        public int TableId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public List<TimeSpan> AvailableTimes { get; set; } = new List<TimeSpan>();
    }
}
