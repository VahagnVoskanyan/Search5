using Search_Service.Models;

namespace Search_Service.Data
{
    public interface ICustomerRepoS
    {
        bool SaveChanges();

        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        bool CustomerExists(int externalId);
        void CreateCustomer(Customer cust);
    }
}
