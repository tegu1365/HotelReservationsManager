using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Reservations
{
    public class ReservationsCreateViewModel
    {
        public virtual Room Room { get; set; }
        public virtual Data.Models.User User { get; set; }
        public List<ReservationCreateClientViewModel> CreateClient { get; set; }
        public List<ReservationCreateRoomViewModel> RoomsAdded { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        
        public DateTime AccommodationDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public bool HaveBreakFast { get; set; }
        public bool IsAllInclusive { get; set; }
       
    }
}
