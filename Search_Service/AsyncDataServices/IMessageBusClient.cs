namespace Search_Service.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void SendNameToBus(string name);
    }
}
