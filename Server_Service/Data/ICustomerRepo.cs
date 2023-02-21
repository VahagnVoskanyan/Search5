using Server_Service.Models;

namespace Server_Service.Data
{
    public interface ICustomerRepo
    {
        bool SaveChanges();

        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        IEnumerable<Customer> GetCustomerByName(string name);
        void CreateCustomer(Customer cust);
    }
}
