using Server_Service.Models;

namespace Server_Service.Data
{
    public interface ICustomerRepo
    {
        IEnumerable<Customer> GetAll();

        Customer? GetById(int id);

        IEnumerable<Customer> GetByName(string name);

        IEnumerable<Customer> GetByNameSim(string name);

        void Create(Customer cust);

        bool ExternalIdExists(int externalId);

        bool SaveChanges();
    }
}
