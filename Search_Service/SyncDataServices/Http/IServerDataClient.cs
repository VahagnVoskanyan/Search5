namespace Search_Service.SyncDataServices.Http
{
    public interface IServerDataClient
    {
        Task SendNameToServer(string name);
    }
}
