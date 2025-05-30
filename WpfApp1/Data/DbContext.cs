using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=SEPHIROTH\\SQLEXPRESS;Database=EnterApp;Trusted_Connection=True;TrustServerCertificate=True;User Id=Sephiroth\\Sasha;Password=2206");
        }
    }
}