using Server_Service.Dtos;

namespace Search_Service.EventProcessing
{
    public interface IEventProcessor
    {
        IEnumerable<CustomerPublishDto> ProcessEvent(string message);
    }
}
