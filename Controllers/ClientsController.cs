using HotelReservationsManager.Data;
using HotelReservationsManager.Data.Models;
using HotelReservationsManager.Models.ViewModels.Clients;
using HotelReservationsManager.Models.ViewModels.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace HotelReservationsManager.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MyHRManagerDBContext _context;
        private const int PageSize = 10;

        public ClientsController(MyHRManagerDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All(ClientsAllViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<ClientsViewModel> items = await _context.Clients.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(client => new ClientsViewModel()
            {
                Id = client.Id,
                FullName = $"{client.FirstName} {client.LastName}",
                Email = client.Email,
                IsAdult = client.IsAdult ? "Yes" : "No",
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await _context.Clients.CountAsync() / (double)PageSize);

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientsCreateViewModel clientsCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Client client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = clientsCreateViewModel.FirstName,
                LastName = clientsCreateViewModel.LastName,
                Email = clientsCreateViewModel.Email,
                IsAdult = clientsCreateViewModel.IsAdult
            };

            await this._context.AddAsync(client);
            await this._context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Client client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            ClientsEditViewModel clientsEditViewModel = new ClientsEditViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                IsAdult = client.IsAdult
            };

            return View(clientsEditViewModel);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ClientsEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Client client = await this._context.Clients
                .SingleOrDefaultAsync(client => client.Id == model.Id);
            client.Id = model.Id;
            client.FirstName = model.FirstName;
            client.LastName = model.LastName;
            client.Email = model.Email;
            client.IsAdult = model.IsAdult;
            this._context.Update(client);
            await this._context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Client client = await this._context.Clients
                .Include(client => client.Reservations)
                .SingleOrDefaultAsync(client => client.Id == id);
            List<Reservation> reser = client.Reservations
                .Where(reserv => reserv.Client.Id == client.Id)
                .Select(res => res.Reservation).ToList();
            List<ReservationsViewModel> clientReservations = new List<ReservationsViewModel>();

            foreach (var r in reser)
            {
                ReservationsViewModel clientReservationsA = new ReservationsViewModel()
                {
                    Id = r.Id,
                    RoomNumber = r.Room.number,
                    UserName = r.User.UserName,
                    AccommodationDate = r.AccommodationDate,
                    ReleaseDate = r.ReleaseDate,
                    HaveBreakFast = r.HaveBreakFast ? "Yes" : "No",
                    IsAllInclusive = r.IsAllInclusive ? "Yes" : "No",
                    DueAmount = r.DueAmount
                };
                clientReservations.Add(clientReservationsA);
            }

            ClientsDetailsViewModel clientsDetailsViewModel = new ClientsDetailsViewModel
            {
                FullName = $"{client.FirstName} {client.LastName}",
                Email = client.Email,
                IsAdult = client.IsAdult ? "Yes" : "No",
                Reservations = clientReservations
            };
            return this.View(clientsDetailsViewModel);
        }
        public async Task<IActionResult> Delete(string id)
        {
            Client client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
