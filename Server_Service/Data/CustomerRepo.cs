using Server_Service.Models;

namespace Server_Service.Data
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _context;

        public CustomerRepo(AppDbContext context) //We injected AppDbContext in program.cs ?? (dependency injection)
        {
            _context = context;
        }
        public void CreateCustomer(Customer cust)
        {
            if (cust == null)
            {
                throw new ArgumentNullException(nameof(cust));
            }
            _context.Customers.Add(cust);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Customer> GetCustomerByName(string name)
        {
            return _context.Customers.ToList().FindAll(x => x.Name == name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
