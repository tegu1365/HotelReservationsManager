using Microsoft.AspNetCore.Identity;
using System;

namespace HotelReservationsManager.Data.Models
{
    public class User : IdentityUser<string>
    {
        public string FirstName { get; set;}
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string UCN { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfDismissal { get; set; }
    }
}
