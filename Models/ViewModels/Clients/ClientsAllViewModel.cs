using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace HotelReservationsManager.Models.ViewModels.Clients
{
    public class ClientsAllViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<ClientsViewModel> Items { get; set; }
    }
}
