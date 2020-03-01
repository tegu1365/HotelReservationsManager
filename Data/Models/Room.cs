using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationsManager.Data.Models
{
    public class Room
    {
        public string Id { get; set; }
        public int Capasity { get; set; }
        public TypeRoom type { get; set; }
        public bool IsFree { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PriceForAdult { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PriceForKid { get; set; }
        public string number { get; set; }

    }
}
