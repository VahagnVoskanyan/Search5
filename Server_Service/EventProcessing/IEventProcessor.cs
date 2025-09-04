using Server_Service.Dtos;

namespace Server_Service.EventProcessing
{
    public interface IEventProcessor
    {
        IEnumerable<CustomerPublishDto> ProcessEvent(string message);
    }
}
