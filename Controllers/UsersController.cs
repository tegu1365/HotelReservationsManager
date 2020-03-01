using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelReservationsManager.Data;
using HotelReservationsManager.Data.Models;
using HotelReservationsManager.Models.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Web.Models.Shared;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationsManager.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyHRManagerDBContext _context;
        private int PageSize = 10;

        public UsersController(MyHRManagerDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> All(UserAllViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<UsersViewModel> items = await _context.Users.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(user => new UsersViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                FullName = $"{user.FirstName} {user.SecondName} {user.LastName}",
                UCN = user.UCN,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                AppointmentDate = user.AppointmentDate,
                IsActive = user.IsActive ? "Yes" : "No",
                DateOfDismissal = (user.DateOfDismissal == new DateTime()) ? "-" : user.DateOfDismissal.ToString("mm/dd/yyyy")

            }).ToListAsync();

            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(await _context.Users.CountAsync() / (double)PageSize);

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            User user = await this._context.Users
                .SingleOrDefaultAsync(user => user.Id == id);

            UsersDetailsViewModel usersDetailsViewModel = new UsersDetailsViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                FullName = $"{user.FirstName} {user.SecondName} {user.LastName}",
                UCN = user.UCN,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                AppointmentDate = user.AppointmentDate,
                IsActive = user.IsActive ? "Yes" : "No",
                DateOfDismissal = (user.DateOfDismissal == new DateTime()) ? "-" : user.DateOfDismissal.ToString("mm/dd/yyyy")
            };

            return this.View(usersDetailsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UsersEditViewModel usersEditViewModel = new UsersEditViewModel
            {
                Id = user.Id,
                Username=user.UserName,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                UCN = user.UCN,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                AppointmentDate = user.AppointmentDate,
                IsActive = user.IsActive,
                DateOfDismissal = user.DateOfDismissal
            };

            return View(usersEditViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(UsersEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await this._context.Users
               .SingleOrDefaultAsync(user => user.Id == model.Id);

                user.UserName = model.Username;
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.LastName = model.LastName;
                user.UCN = model.UCN;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.AppointmentDate = model.AppointmentDate;
                user.IsActive = model.IsActive;
                user.DateOfDismissal = model.DateOfDismissal;

                try
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            User user = await _context.Users.FindAsync(id);
            user.IsActive = false;
            user.DateOfDismissal = DateTime.Today;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}