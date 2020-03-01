using HotelReservationsManager.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationsManager.Models.ViewModels.Rooms
{
    public class RoomsEditViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Negative values or 0 are not accepted")]
        public int Capasity { get; set; }
        [Required]
        [EnumDataType(typeof(TypeRoom))]
        public TypeRoom type { get; set; }
        [Required]
        public bool IsFree { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Negative values are not accepted")]
        public decimal PriceForAdult { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Negative values are not accepted")]
        public decimal PriceForKid { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "Must be at least 2 characters long. Cannot be more than 6 characters")]
        public string number { get; set; }
        [HiddenInput]
        public string Id { get;set; }
    }
}
