using CarRental.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserVerificationCode> VerificationCodes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImages> CarImages { get; set; }
        public DbSet<Rental> Rentals { get; set; }
    }
}
