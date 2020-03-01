using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.Shared
{
    public enum RoomType
    {
        [Description("Two Single Beds")]
        TwoSingleBeds,
        Apartment,
        [Description("Double Bed Room")]
        DoubleBedRoom,
        Penthouse,
        Maisonette
    }
}
