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
                    new Customer() { Name = "Tigran", Surname = "Hovsepyan", City = "London", Email = "THovsepyan@gmail.com" },
                    new Customer() { Name = "Hovsep", Surname = "Mkrtchyan", City = "Milan", Email = "HMkrtchyan@gmail.com" },

                    new Customer() { Name = "Arman", Surname = "Simonyan", City = "Rome", Email = "ASimonyan@gmail.com" },
                    new Customer() { Name = "Arsen", Surname = "Hovsepyan", City = "Madrid", Email = "AHovsepyan@gmail.com" },
                    new Customer() { Name = "Artash", Surname = "Mkrtchyan", City = "Berlin", Email = "AMkrtchyan@gmail.com" },
                    new Customer() { Name = "Ararat", Surname = "Simonyan", City = "Vienna", Email = "ArSimonyan@gmail.com" },
                    new Customer() { Name = "Artyom", Surname = "Tadevosyan", City = "Sofia", Email = "ATadevosyan@gmail.com" },
                    new Customer() { Name = "A", Surname = "B", City = "C", Email = "AB@gmail.com" }
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
