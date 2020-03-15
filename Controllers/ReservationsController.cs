using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservationsManager.Data;
using HotelReservationsManager.Models.ViewModels.Reservations;
using Web.Models.Shared;
using HotelReservationsManager.Models.ViewModels.Clients;

namespace HotelReservationsManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly MyHRManagerDBContext _context;

        private const int PageSize = 10;
        // private const string password = Constants.emailPassword;
        //  private readonly ILogger<ReservationController> _logger;
        private UserManager<User> UserManager { get; set; }

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

            List<ReservationCreateClientViewModel> clients = _context.Clients
                .Select(x => new ReservationCreateClientViewModel(x.Id, $"{x.FirstName} {x.LastName}"))
                .ToList();

            List<ReservationCreateRoomViewModel> rooms = _context.Rooms.Select(x => new ReservationCreateRoomViewModel(x.Id, x.number, x.type, x.PriceForAdult, x.PriceForKid, x.IsFree)).ToList();

            List<ReservationCreateRoomViewModel> freeRooms = new List<ReservationCreateRoomViewModel>();

            foreach (var room in rooms)
            {
                if (room.IsFree)
                {
                    freeRooms.Add(room);
                }
            }

            model.CreateClient = clients;
            model.RoomsAdded = freeRooms;

            return View(model);
        }

        // POST: Reservation/Create        
        [HttpPost]
        public async Task<IActionResult> Create(RequestReservationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User user = _context.Users.FirstOrDefault(x => x.Id == userId);
            decimal kids = 0;
            decimal adults = 0;

            List<Client> clients = new List<Client>();

            foreach (var clientId in model.ClientsId)
            {
                Client client = new Client();
                client = _context.Clients.FirstOrDefault(x => x.Id == clientId);
                if (client.IsAdult)
                {
                    adults++;
                }
                else
                {
                    kids++;
                }
                clients.Add(client);
            }

            Room room = _context.Rooms.FirstOrDefault(x => x.Id == model.RoomId);
            room.IsFree = false;
            var days = (decimal)(model.ReleaseDate.Subtract(model.AccommodationDate.Date).TotalDays);
            decimal dueAm = days * (adults * room.PriceForAdult + kids * room.PriceForKid);


            Reservation reservation = new Reservation
            {
                Id = Guid.NewGuid().ToString(),
                User = user,
                AccommodationDate = model.AccommodationDate,
                ReleaseDate = model.ReleaseDate,
                HaveBreakFast = model.HaveBreakFast,
                IsAllInclusive = model.IsAllInclusive,
                DueAmount = dueAm,
                Room = room,
                Clients = clients.Select(client => new ClientReservation { Id = Guid.NewGuid().ToString(), Client = client }).ToList()
            };

            // reservation.Clients = clients.Select(client => new ClientReservation { Id = Guid.NewGuid().ToString(), Client = client, Reservation = reservation }).ToList();

            await this._context.AddAsync(reservation);
            await this._context.SaveChangesAsync();

            return RedirectToAction(nameof(All));



        }



        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation reservation = await _context.Reservations
                .Include(room => room.Room)
                .Include(user => user.User)
                .SingleOrDefaultAsync(rese => rese.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            List<ReservationCreateRoomViewModel> rooms = _context.Rooms.Select(x => new ReservationCreateRoomViewModel(x.Id, x.number, x.type, x.PriceForAdult, x.PriceForKid, x.IsFree)).ToList();

            List<ReservationCreateRoomViewModel> freeRooms = new List<ReservationCreateRoomViewModel>();
            freeRooms.Add(new ReservationCreateRoomViewModel(reservation.Room.Id, reservation.Room.number, reservation.Room.type, reservation.Room.PriceForAdult, reservation.Room.PriceForKid, reservation.Room.IsFree));
            foreach (var room in rooms)
            {
                if (room.IsFree)
                {
                    freeRooms.Add(room);
                }
            }
            ReservationsEditViewModel model = new ReservationsEditViewModel
            {
                Id = reservation.Id,
                User = reservation.User,
                Room = reservation.Room,
                AccommodationDate = reservation.AccommodationDate,
                ReleaseDate = reservation.ReleaseDate,
                HaveBreakFast = reservation.HaveBreakFast,
                IsAllInclusive = reservation.IsAllInclusive
            };
            model.RoomsAdded = freeRooms;

            Room r = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == reservation.Room.Id);
            r.IsFree = true;
            _context.Update(r);
            await _context.SaveChangesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReservationsEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.FirstOrDefault(x => x.Id == userId);
            Room room = _context.Rooms.FirstOrDefault(x => x.Id == model.RoomId);
            room.IsFree = false;

            
            decimal adults = 0, kids = 0;
            List<Client> clients = new List<Client>();
            Reservation reser = await this._context.Reservations
                .Include(res => res.Clients)
                .ThenInclude(client => client.Client)
                .SingleOrDefaultAsync(rese => rese.Id == model.Id);
            List<ClientReservation> cl = _context.ClientReservations.Where(res => res.Reservation.Id == reser.Id).ToList();
            List<string> clId = new List<string>();

            foreach (var c in cl)
            {
                clId.Add(c.Client.Id);
            }

            foreach (var c in clId)
            {
                Client client = new Client();
                client = _context.Clients.FirstOrDefault(x => x.Id == c);
                clients.Add(client);
            }
            foreach (var client in clients)
            {
                if (client.IsAdult)
                {
                    adults++;
                }
                else
                {
                    kids++;
                }
            }
            var days = (decimal)(model.ReleaseDate.Subtract(model.AccommodationDate.Date).TotalDays);
            decimal dueAm = days * (adults * room.PriceForAdult + kids * room.PriceForKid);
            reser.Id = model.Id;
            reser.User = user;
            reser.Room = room;
            reser.AccommodationDate = model.AccommodationDate;
            reser.ReleaseDate = model.ReleaseDate;
            reser.HaveBreakFast = model.HaveBreakFast;
            reser.IsAllInclusive = model.IsAllInclusive;
            reser.DueAmount = dueAm;


            _context.Update(reser);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(All));



        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Reservation r = await this._context.Reservations
                .Include(room => room.Room)
                .Include(user => user.User)
                .Include(clients => clients.Clients)
                .ThenInclude(x=>x.Client)
                .SingleOrDefaultAsync(r => r.Id == id);
            List<ClientsViewModel> clients = new List<ClientsViewModel>();
            List<ClientReservation> ch = _context.ClientReservations.Where(res => res.Reservation.Id == id).ToList();
            List<string> chId = new List<string>();
            List<Client> client = new List<Client>();

            foreach (var c in ch)
            {
                chId.Add(c.Client.Id);
            }

            foreach (var c in chId)
            {
                Client client1 = new Client();
                client1 = _context.Clients.FirstOrDefault(x => x.Id == c);
                client.Add(client1);
            }

            foreach (var c in client)
            {
                ClientsViewModel clientReservationsA = new ClientsViewModel()
                {
                    Id = c.Id,
                    FullName = $"{c.FirstName} {c.LastName}",
                    Email = c.Email,
                    IsAdult = c.IsAdult ? "Yes" : "No",
                };
                clients.Add(clientReservationsA);
            }

            ReservationsDetailsViewModel reservationsDetailsViewModel = new ReservationsDetailsViewModel
            {
                Id = r.Id,
                UserName = r.User.UserName,
                RoomNumber=r.Room.number,
                AccommodationDate = r.AccommodationDate,
                ReleaseDate = r.ReleaseDate,
                HaveBreakFast = r.HaveBreakFast ? "Yes" : "No",
                IsAllInclusive = r.IsAllInclusive ? "Yes" : "No",
                DueAmount = r.DueAmount,
                Clients = clients
            };

           
            return this.View(reservationsDetailsViewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Reservation reservation = await _context.Reservations
                .Include(room=>room.Room)
                .Include(client=>client.Clients)
                .SingleOrDefaultAsync(rese => rese.Id == id);
            reservation.Room.IsFree = true;
            foreach(var res in reservation.Clients) 
            {
                _context.ClientReservations.Remove(res);
            }    
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        private bool ReservationExists(string id)
        {
            return _context.Reservations.Any(r => r.Id == id);
        }
    }
}
