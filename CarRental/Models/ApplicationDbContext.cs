using CarRental.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
