using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Rooms
{
    public class RoomsViewModel
    {
        public string Id { get; set; }
        public int Capasity { get; set; }
        public string type { get; set; }
        public string IsFree { get; set; }
        public decimal PriceForAdult { get; set; }
        public decimal PriceForKid { get; set; }
        public string number { get; set; }
    }
}
