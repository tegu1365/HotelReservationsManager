using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Data.Models
{
    public class ClientReservation
    {
        public string Id { get; set; }
        //public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        //public int ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
