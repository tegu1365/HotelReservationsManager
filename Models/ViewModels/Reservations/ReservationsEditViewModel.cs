using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Reservations
{
    public class ReservationsEditViewModel
    {
        public string Id { get;set; }
        public virtual Room Room { get; set; }
        public List<ReservationCreateRoomViewModel> RoomsAdded { get; set; }
        public string RoomId { get; set; }
        public virtual Data.Models.User User { get; set; }
        [DataType(DataType.Date)]
        public DateTime AccommodationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public bool HaveBreakFast { get; set; }
        public bool IsAllInclusive { get; set; }
        public decimal DueAmount { get; set; }
    }
}
