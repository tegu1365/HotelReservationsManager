using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace HotelReservationsManager.Models.ViewModels.Rooms
{
    public class RoomAllViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<RoomsViewModel> Items { get; set; }
    }
}
