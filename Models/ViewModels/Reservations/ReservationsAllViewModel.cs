using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace HotelReservationsManager.Models.ViewModels.Reservations
{
    public class ReservationsAllViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<ReservationsViewModel> Items { get; set; }
    }
}
