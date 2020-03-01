using HotelReservationsManager.Data;
using HotelReservationsManager.Data.Models;
using HotelReservationsManager.Models.ViewModels.Reservations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace HotelReservationsManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly MyHRManagerDBContext _context;
        private const int PageSize = 10;

        public ReservationsController(MyHRManagerDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All(ReservationsAllViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<ReservationsViewModel> items = await _context.Reservations.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(reservation => new ReservationsViewModel()
            {
                Id = reservation.Id,
                RoomNumber = reservation.Room.number,
                UserName = reservation.User.UserName,
                AccommodationDate = reservation.AccommodationDate,
                ReleaseDate = reservation.ReleaseDate,
                HaveBreakFast = reservation.HaveBreakFast ? "Yes" : "No",
                IsAllInclusive = reservation.IsAllInclusive ? "Yes" : "No",
                DueAmount = reservation.DueAmount
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await _context.Reservations.CountAsync() / (double)PageSize);

            return View(model);
        }

        public IActionResult Create()
        {
            ReservationsCreateViewModel model = new ReservationsCreateViewModel();
            //SelectList clients = new SelectList(Client, "Id", "Name");
            //TempData["Cities"] = cityList;

            return View(model);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                int kids = 0;
                int adults = 0;
                List<ClientReservation> clientReservation = new List<ClientReservation>();
                Reservation reservation = new Reservation
                {
                    Id = Guid.NewGuid().ToString(),
                    AccommodationDate = model.AccommodationDate,
                    ReleaseDate = model.ReleaseDate,
                    HaveBreakFast = model.HaveBreakFast,
                    IsAllInclusive = model.IsAllInclusive,
                    Room = model.Room,
                    User = model.User
                };
                foreach (var cl in model.Clients)
                {
                    if (cl.IsAdult)
                    {
                        adults++;
                    }
                    else
                    {
                        kids++;
                    }
                }

                reservation.DueAmount = (decimal)(model.ReleaseDate.Subtract(model.AccommodationDate.Date).TotalDays)
                    * (adults * model.Room.PriceForAdult + kids * model.Room.PriceForKid);
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Reservation reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }
    }
}
