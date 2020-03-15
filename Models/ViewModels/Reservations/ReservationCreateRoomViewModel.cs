using HotelReservationsManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Reservations
{
    public class ReservationCreateRoomViewModel
    {
        public string Id { get; set; }
        public string number { get; set; }

        public TypeRoom RoomType { get; set; }

        public decimal PriceForAdult { get; set; }

        public decimal PriceForKid { get; set; }

        public bool IsFree { get; set; }

        public ReservationCreateRoomViewModel(string id, string num,TypeRoom roomType, decimal pricePerAdult, decimal pricePerChild, bool isFree)
        {
            this.Id = id;
            this.number = num;
            this.RoomType = roomType;
            this.PriceForAdult = pricePerAdult;
            this.PriceForKid = pricePerChild;
            this.IsFree = isFree;
        }
    }
}
