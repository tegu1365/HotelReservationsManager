using HotelReservationsManager.Data;
using HotelReservationsManager.Data.Models;
using HotelReservationsManager.Models.ViewModels.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace HotelReservationsManager.Controllers
{
    public class RoomsController : Controller
    {
        private readonly MyHRManagerDBContext _context;
        private const int PageSize = 10;

        public RoomsController(MyHRManagerDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All(RoomAllViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<RoomsViewModel> items = await _context.Rooms.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(room => new RoomsViewModel()
            {
                Id = room.Id,
                Capasity = room.Capasity,
                type = room.type.ToString(),
                IsFree = room.IsFree ? "Yes" : "No",
                PriceForAdult = room.PriceForAdult,
                PriceForKid = room.PriceForKid,
                number = room.number
            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await _context.Rooms.CountAsync() / (double)PageSize);

            return View(model);
        }

        public IActionResult Create()
        {
            RoomsCreateViewModel model = new RoomsCreateViewModel();

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RoomsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Room room = new Room
                {
                    Capasity = model.Capasity,
                    number = model.number,
                    type = model.type,
                    PriceForAdult = model.PriceForAdult,
                    PriceForKid = model.PriceForKid,
                    IsFree=model.IsFree,
                };
                room.Id = Guid.NewGuid().ToString();
                _context.Add(room);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(All));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Room room = await this._context.Rooms
                .SingleOrDefaultAsync(room => room.Id == id);

            RoomsDetailsViewModel roomsDetailsViewModel = new RoomsDetailsViewModel
            {
                Id = room.Id,
                Capasity = room.Capasity,
                type = room.type.ToString(),
                IsFree = room.IsFree ? "Yes" : "No",
                PriceForAdult = room.PriceForAdult,
                PriceForKid = room.PriceForKid,
                Number = room.number
            };

            return this.View(roomsDetailsViewModel);
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            RoomsEditViewModel model = new RoomsEditViewModel
            {
                Id=room.Id,
                Capasity = room.Capasity,
                number = room.number,
                type = room.type,
                PriceForAdult = room.PriceForAdult,
                PriceForKid = room.PriceForKid,
                IsFree = room.IsFree,
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomsEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Room room = new Room
                {
                    Id = model.Id,
                    Capasity = model.Capasity,
                    number = model.number,
                    type = model.type,
                    PriceForAdult = model.PriceForAdult,
                    PriceForKid = model.PriceForKid,
                    IsFree = model.IsFree,
                };

                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(All));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Room room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }
        private bool RoomExists(string id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Search(string searched)
        {
            var room = from n in _context.Rooms
                       select n;

            if (!String.IsNullOrEmpty(searched))
            {
                room = room.Where(x => x.number.Contains(searched));
            }

            return View(await room.ToListAsync());
        }
    }
}
