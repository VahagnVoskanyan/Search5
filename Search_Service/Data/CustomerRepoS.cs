using Search_Service.Models;

namespace Search_Service.Data
{
    public class CustomerRepoS : ICustomerRepoS
    {
        private readonly AppDbContext _context;

        public CustomerRepoS(AppDbContext context)
        {
            _context = context;
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public bool CustomerExists(int externalId)
        {
            return _context.Customers.Any(x => x.ExternalId == externalId);
        }

        public void CreateCustomer(Customer cust)
        {
            _context.Customers.Add(cust);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
