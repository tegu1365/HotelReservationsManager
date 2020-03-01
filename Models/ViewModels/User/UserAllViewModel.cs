using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace HotelReservationsManager.Models.ViewModels.User {
    public class UserAllViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<UsersViewModel> Items { get; set; }
    }
}
