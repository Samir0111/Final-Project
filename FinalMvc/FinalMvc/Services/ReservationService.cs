using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Table>> GetAvailableTablesAsync(DateTime date, int guestCount)
        {
            var allTables = await _context.Tables
                .Where(t => t.GuestCapacity >= guestCount &&
                            t.AvailabilityStart.HasValue && t.AvailabilityEnd.HasValue &&
                            t.AvailabilityStart.Value.Date <= date.Date &&
                            t.AvailabilityEnd.Value.Date >= date.Date)
                .ToListAsync();

            var reservedTableIds = await _context.Reservations
                .Where(r => r.Date.Date == date.Date)
                .Select(r => r.TableId)
                .Distinct()
                .ToListAsync();

            return allTables.Where(t => !reservedTableIds.Contains(t.Id)).ToList();
        }

        public async Task<bool> IsTableAvailableAsync(int tableId, DateTime date, TimeSpan reserveTime)
        {
            return !await _context.Reservations.AnyAsync(r =>
                r.TableId == tableId &&
                r.Date == date &&
                r.ReserveTime.TimeOfDay == reserveTime);
        }

        public async Task<List<TimeSpan>> GetAvailableTimeSlotsAsync(int tableId, DateTime date)
        {
            var allTimeSlots = new List<TimeSpan>();
            for (int hour = 1; hour <= 3; hour++)
            {
                allTimeSlots.Add(new TimeSpan(hour, 0, 0));
            }

            var reservedTimes = await _context.Reservations
                .Where(r => r.TableId == tableId && r.Date == date)
                .Select(r => r.ReserveTime.TimeOfDay)
                .ToListAsync();

            return allTimeSlots.Where(t => !reservedTimes.Contains(t)).ToList();
        }

        public async Task<Reservation> ReserveTableAsync(Reservation reservation, AppUser user)
        {
            bool isAlreadyReserved = await _context.Reservations.AnyAsync(r =>
                r.TableId == reservation.TableId &&
                r.Date == reservation.Date &&
                r.ReserveTime == reservation.ReserveTime);

            if (isAlreadyReserved)
            {
                throw new InvalidOperationException("This table is already reserved at the selected time.");
            }

            reservation.UserId = user.Id;
            reservation.AppUser = user;
            reservation.Status = "Pending";

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetUserReservationsAsync(string userId)
        {
            return await _context.Reservations
                .Include(r => r.Table)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsAsync()
        {
            return await _context.Reservations
                                 .Include(r => r.Table)
                                 .ToListAsync() ?? new List<Reservation>();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.AppUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            var existingReservation = await _context.Reservations.FindAsync(reservation.Id);
            if (existingReservation == null)
                throw new InvalidOperationException("Reservation not found.");

            bool isAlreadyReserved = await _context.Reservations.AnyAsync(r =>
                r.TableId == reservation.TableId &&
                r.Date == reservation.Date &&
                r.ReserveTime == reservation.ReserveTime &&
                r.Id != reservation.Id);

            if (isAlreadyReserved)
            {
                throw new InvalidOperationException("This table is already reserved at the selected time.");
            }

            existingReservation.Date = reservation.Date;
            existingReservation.ReserveTime = reservation.ReserveTime;
            existingReservation.EndReserveTime = reservation.EndReserveTime;
            existingReservation.Status = reservation.Status;

            _context.Reservations.Update(existingReservation);
            await _context.SaveChangesAsync();
        }
    }
}