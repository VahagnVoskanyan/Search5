using Search_Service.Models;

namespace Search_Service.SyncDataServices.gRPC
{
    public interface IGrpcDataClient
    {
        IEnumerable<Customer> GetCustByName(string name);
    }
}
