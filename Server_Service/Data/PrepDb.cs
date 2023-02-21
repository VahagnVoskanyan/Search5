using Server_Service.Models;

namespace Server_Service.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        private static void SeedData(AppDbContext context)
        {
            if (!context.Customers.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Customers.AddRange(
                    new Customer() { Name = "Armen", Surname = "Ananyan", City = "Yerevan", Email = "AAnanyan@gmail.com" },
                    new Customer() { Name = "Armen", Surname = "Barseghyan", City = "Ijevan", Email = "ABarseghyan@gmail.com" },
                    new Customer() { Name = "Tigran", Surname = "Hovsepyan", City = "London", Email = "THovsefyan@gmail.com" },
                    new Customer() { Name = "Hovsep", Surname = "Mkrtchyan", City = "Milan", Email = "HMkrtchyan@gmail.com" }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
