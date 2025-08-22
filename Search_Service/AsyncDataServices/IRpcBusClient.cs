namespace Search_Service.AsyncDataServices
{
    public interface IRpcBusClient
    {
        Task<string> SendNameToBusAsync(string name, CancellationToken cancellationToken = default);
    }
}
