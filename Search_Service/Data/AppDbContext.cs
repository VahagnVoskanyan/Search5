using Microsoft.EntityFrameworkCore;
using Search_Service.Models;

namespace Search_Service.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public DbSet<Customer> Customers { get; set; }
    }
}
