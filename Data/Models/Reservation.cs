using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationsManager.Data.Models
{
    public class Reservation
    {
        public string Id { get; set; }
        public Room Room { get; set; }
        public User User { get; set; }
        public ICollection<ClientReservation> Clients { get; set; }
        public DateTime AccommodationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool HaveBreakFast { get; set; }
        public bool IsAllInclusive { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal DueAmount { get; set; }

        public Reservation()
        { }

       
    }
}
