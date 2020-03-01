using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Rooms { 
    public class RoomsDetailsViewModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public int Capasity { get; set; }
        public string type { get; set; }
        public string IsFree { get; set; }
        public decimal PriceForAdult { get; set; }
        public decimal PriceForKid { get; set; }
       
    }
}
