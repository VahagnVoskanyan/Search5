using Search_Service.Dtos;

namespace Search_Service.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void SendNewCustomer(CustomerPublishDto customerPublishDto);
    }
}
