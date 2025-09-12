using Server_Service.Dtos;

namespace Server_Service.EventProcessing
{
    public interface IEventProcessor
    {
        IEnumerable<CustomerPublishedDto> ProcessEvent(string message);
    }
}
