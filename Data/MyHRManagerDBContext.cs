using HotelReservationsManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Data
{
    public class MyHRManagerDBContext : IdentityDbContext<User, IdentityRole, string>
    {
      //  public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientReservation> ClientReservations { get; set; }

        public MyHRManagerDBContext(DbContextOptions<MyHRManagerDBContext> options)
              : base(options)
        {

        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(@"Server=localhost;Database=HotelReaservationsDB;Initial Catalog=HotelReaservationsDB;Trusted_Connection=True");
        }
    }
}
