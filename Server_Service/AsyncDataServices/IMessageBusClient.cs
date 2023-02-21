using Server_Service.Dtos;

namespace Server_Service.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishCustomerByName(IEnumerable<CustomerPublishDto> customerPublishDto);
    }
}
