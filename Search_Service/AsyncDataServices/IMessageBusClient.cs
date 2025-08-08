namespace Search_Service.AsyncDataServices
{
    public interface IMessageBusClient
    {
        Task<string> SendNameToBusAsync(string name, CancellationToken cancellationToken = default);
    }
}
