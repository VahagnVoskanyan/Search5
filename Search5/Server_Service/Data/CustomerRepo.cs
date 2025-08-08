using Server_Service.Models;
using FuzzySharp.SimilarityRatio; //For similar matching
using FuzzySharp;

namespace Server_Service.Data
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _context;

        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Customer cust)
        {
            _context.Customers.Add(cust);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer? GetById(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Customer> GetByName(string name)
        {
            return _context.Customers.ToList().FindAll(x => x.Name == name);
        }

        //Where similarity is above 50%
        public IEnumerable<Customer> GetByNameSim(string name)
        {
            return _context.Customers.ToList().FindAll(x => Fuzz.Ratio(x.Name,name) > 50);
        }
        
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
