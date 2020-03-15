using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Reservations
{
    public class RequestReservationCreateViewModel
    {
        public virtual Room Room { get; set; }

        public virtual Data.Models.User User { get; set; }

        public List<string> ClientsId { get; set; }

        public string RoomId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AccommodationDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public bool HaveBreakFast { get; set; }

        public bool IsAllInclusive { get; set; }

        public decimal DueAmount { get; set; }
    }
}
