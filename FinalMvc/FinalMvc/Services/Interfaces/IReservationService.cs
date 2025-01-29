using FinalMvc.Models;

namespace FinalMvc.Services.Interfaces
{
    //public interface IReservationService
    //{
    //    Task<List<Table>> GetAvailableTablesAsync(DateTime date, int guestCount);

    //    Task<List<TimeSpan>> GetAvailableTimeSlotsAsync(int tableId, DateTime date);

    //    Task<Reservation> ReserveTableAsync(Reservation reservation, AppUser user);

    //    Task<IEnumerable<Reservation>> GetUserReservationsAsync(string userId);

    //    Task<IEnumerable<Reservation>> GetReservationsAsync();

    //    Task<Reservation> GetReservationByIdAsync(int id);

    //    Task DeleteReservationAsync(int id);

    //    Task UpdateReservationAsync(Reservation reservation);
    //}

    public interface IReservationService
    {
        Task<List<Table>> GetAvailableTablesAsync(DateTime date, int guestCount);
        Task<bool> IsTableAvailableAsync(int tableId, DateTime date, TimeSpan reserveTime);
        Task<Reservation> ReserveTableAsync(Reservation reservation, AppUser user);
        Task<IEnumerable<Reservation>> GetUserReservationsAsync(string userId);
        Task<List<Reservation>> GetReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task DeleteReservationAsync(int id);
        Task UpdateReservationAsync(Reservation reservation);


    }


}
