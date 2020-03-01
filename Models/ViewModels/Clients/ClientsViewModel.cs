using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Clients
{
    public class ClientsViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IsAdult { get; set; }
        //public ICollection<ClientReservation> reservations { get; set; }
    }
}
