using HotelReservationsManager.Data.Models;
using HotelReservationsManager.Models.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Reservations
{
    public class ReservationsDetailsViewModel
    {
        public string Id { get; set; }
        public string RoomNumber { get; set; }
        public string UserName { get; set; }
        public ICollection<ClientsViewModel> Clients { get; set; }
        [DataType(DataType.Date)]
        public DateTime AccommodationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string HaveBreakFast { get; set; }
        public string IsAllInclusive { get; set; }
        public decimal DueAmount { get; set; }
    }
}
