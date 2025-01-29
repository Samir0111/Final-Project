using FinalMvc.Models;
using FinalMvc.Services.Interfaces;
using FinalMvc.ViewModels;
using FinalMvc.ViewModels.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Globalization;

namespace FinalMvc.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly UserManager<AppUser> _userManager;

        public ReservationController(IReservationService reservationService, UserManager<AppUser> userManager)
        {
            _reservationService = reservationService;
            _userManager = userManager;
        }

        // ✅ 1️⃣ Get Available Tables Based on Date & Guest Count
        [HttpGet]
        public async Task<IActionResult> AvailableTables(DateTime date, int guestCount)
        {
            var model = new ReservePageViewModel
            {
                AvailableTables = new List<TableVM>(),
                Reservation = new TableReservationViewModel
                {
                    Date = date,
                    GuestCount = guestCount
                }
            };

            if (date == DateTime.MinValue || guestCount <= 0)
            {
                ModelState.AddModelError("", "Please provide a valid date and guest count.");
                return View(model);
            }

            var tables = await _reservationService.GetAvailableTablesAsync(date, guestCount);
            model.AvailableTables = tables.Select(t => new TableVM
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                GuestCapacity = t.GuestCapacity
            }).ToList();

            return View(model);
        }

        // ✅ 2️⃣ Reserve a Table
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Reserve(int tableId, DateTime date, string reserveTime, int guestCount)
        {
            var table = (await _reservationService.GetAvailableTablesAsync(date, guestCount))
                        .FirstOrDefault(t => t.Id == tableId);

            if (table == null)
            {
                ModelState.AddModelError("", "The selected table is not available.");
                return RedirectToAction(nameof(AvailableTables), new { date, guestCount });
            }

            if (!TimeSpan.TryParseExact(reserveTime, "hh\\:mm", CultureInfo.InvariantCulture, out var parsedReserveTime))
            {
                ModelState.AddModelError("", "Invalid time format.");
                return RedirectToAction(nameof(AvailableTables), new { date, guestCount });
            }

            // ✅ Check if the table is already reserved at the selected time
            var isAvailable = await _reservationService.IsTableAvailableAsync(table.Id, date, parsedReserveTime);
            if (!isAvailable)
            {
                ModelState.AddModelError("", "This table is already reserved at the selected time.");
                return RedirectToAction(nameof(AvailableTables), new { date, guestCount });
            }

            var startTime = date.Date.Add(parsedReserveTime);
            var endTime = startTime.AddHours(2); // Assuming 2-hour reservations

            var viewModel = new TableReservationViewModel
            {
                TableId = table.Id,
                TableNumber = table.TableNumber,
                GuestCount = guestCount, // ✅ Fix: Preserve Guest Count
                Date = date,
                ReserveTime = startTime,
                EndReserveTime = endTime
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Reserve(TableReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ✅ Phone number validation
            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number is required.");
                return View(model);
            }

            var phoneRegex = @"^\+994(50|51|55|70|77|10)[1-9][0-9]{6}$";
            if (!Regex.IsMatch(model.PhoneNumber, phoneRegex))
            {
                ModelState.AddModelError("PhoneNumber", "Invalid phone number format. Example: +994514443024.");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // ✅ Check if the table is already reserved at this time
            var isTableAvailable = await _reservationService.IsTableAvailableAsync(model.TableId, model.Date, model.ReserveTime.TimeOfDay);
            if (!isTableAvailable)
            {
                ModelState.AddModelError("", "This table is already reserved at the selected time.");
                return View(model);
            }

            // ✅ Reservation logic
            var reservation = new Reservation
            {
                TableId = model.TableId,
                Date = model.Date,
                ReserveTime = model.ReserveTime,
                EndReserveTime = model.EndReserveTime,
                PhoneNumber = model.PhoneNumber,
                Status = "Pending"
            };

            await _reservationService.ReserveTableAsync(reservation, user);

            TempData["SuccessMessage"] = "Your reservation has been submitted and is pending admin approval.";
            return RedirectToAction(nameof(MyReservations));
        }

        // ✅ 3️⃣ View User's Reservations
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var reservations = await _reservationService.GetUserReservationsAsync(user.Id);

            var reservationList = reservations
                .Select(r => new TableReservationViewModel
                {
                    TableId = r.TableId,
                    TableNumber = r.Table.TableNumber,
                    GuestCount = r.Table.GuestCapacity, // ✅ Fix: Ensure correct Guest Count
                    Date = r.Date,
                    ReserveTime = r.ReserveTime,
                    EndReserveTime = r.EndReserveTime,
                    PhoneNumber = r.PhoneNumber,

                    Status = r.Status
                })
                .ToList();

            var viewModel = new ReservePageViewModel
            {
                ReservationList = reservationList
            };

            return View(viewModel);
        }
    }
}
