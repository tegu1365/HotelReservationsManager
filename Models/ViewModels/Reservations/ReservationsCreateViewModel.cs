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
        public string Id { get; set; }
        [Required]
       
        public Room Room { get; set; }
        [Required]
        public Data.Models.User User { get; set; }
        [Required]
        public ICollection<Client> Clients { get; set; }
        [DataType(DataType.Date)]
        public DateTime AccommodationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public bool HaveBreakFast { get; set; }
        public bool IsAllInclusive { get; set; }
       
    }
}
