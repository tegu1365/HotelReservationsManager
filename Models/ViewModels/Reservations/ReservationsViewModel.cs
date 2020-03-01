using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Reservations
{
    public class ReservationsViewModel
    {
        public string Id { get; set; }
        public string RoomNumber { get; set; }
        public string UserName { get; set; }
         public ICollection<ClientReservation> Clients { get; set; }
        [DataType(DataType.Date)]
        public DateTime AccommodationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string HaveBreakFast { get; set; }
        public string IsAllInclusive { get; set; }
        public decimal DueAmount { get; set; }
    }
}
