using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HotelReservationsManager.Models;
using HotelReservationsManager.Data;


namespace HotelReservationsManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyHRManagerDBContext context;

        public HomeController(MyHRManagerDBContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.View("Home");
            }
            if(!context.Users.Any())
            {
                return this.View("NoUsers");
            }

            return View();
        }
    }
}
