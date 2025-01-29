using System.Collections.Generic;

namespace FinalMvc.ViewModels.Reservation
{
    public class ReservePageViewModel
    {
        public List<TableVM> AvailableTables { get; set; } = new List<TableVM>();

        public TableReservationViewModel Reservation { get; set; } = new TableReservationViewModel();

        public List<TableReservationViewModel> ReservationList { get; set; } = new List<TableReservationViewModel>();
    }
}
