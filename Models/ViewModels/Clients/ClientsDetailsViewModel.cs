using HotelReservationsManager.Data.Models;
using HotelReservationsManager.Models.ViewModels.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Clients
{
    public class ClientsDetailsViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IsAdult { get; set; }
        public ICollection<ReservationsViewModel> Reservations { get; set; }
    }
}
