using FinalMvc.Models;
using FinalMvc.ViewModels.Admin.Reservation;
using FinalMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // ✅ 1️⃣ Display All Reservations in Admin Panel
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetReservationsAsync() ?? new List<Reservation>();

            var reservationVMs = reservations.Select(r => new ReservationVM
            {
                Id = r.Id,
                TableNumber = r.Table?.TableNumber ?? 0, 
                Date = r.Date,
                ReserveTime = r.ReserveTime,
                EndReserveTime = r.EndReserveTime,
                PhoneNumber = r.PhoneNumber,
                Status = r.Status
            }).ToList();

            return View(reservationVMs);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            var reservationVM = new ReservationVM
            {
                Id = reservation.Id,
                TableId = reservation.TableId,
                TableNumber = reservation.Table.TableNumber,
                PhoneNumber = reservation.PhoneNumber,
                Date = reservation.Date,
                ReserveTime = reservation.ReserveTime,
                EndReserveTime = reservation.EndReserveTime,
                Status = reservation.Status
            };

            return View(reservationVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationService.DeleteReservationAsync(id);
            TempData["SuccessMessage"] = "Reservation deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            reservation.Status = "Approved";
            await _reservationService.UpdateReservationAsync(reservation);

            TempData["SuccessMessage"] = "Reservation approved successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Decline(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            reservation.Status = "Declined";
            await _reservationService.UpdateReservationAsync(reservation);

            TempData["SuccessMessage"] = "Reservation declined successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            var reservationVM = new ReservationVM
            {
                Id = reservation.Id,
                TableId = reservation.TableId,
                TableNumber = reservation.Table.TableNumber,
                PhoneNumber = reservation.PhoneNumber,
                Date = reservation.Date,
                ReserveTime = reservation.ReserveTime,
                EndReserveTime = reservation.EndReserveTime,
                Status = reservation.Status
            };

            return View(reservationVM);
        }
    }
}
