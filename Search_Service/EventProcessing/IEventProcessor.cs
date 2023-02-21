namespace Search_Service.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
