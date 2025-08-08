using Microsoft.EntityFrameworkCore;
using Server_Service.Models;

namespace Server_Service.Data
{
    public class AppDbContext : DbContext //Base class is from EntityFramework
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
