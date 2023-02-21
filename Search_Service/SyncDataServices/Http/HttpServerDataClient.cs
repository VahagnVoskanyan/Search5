using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace Search_Service.SyncDataServices.Http
{
    public class HttpServerDataClient : IServerDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpServerDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendNameToServer(string name)
        {
            /*var httpContent = new StringContent(
                JsonSerializer.Serialize(name),
                Encoding.UTF8,
                "application/json");*/
            
            var response = await _httpClient.GetAsync($"{_configuration["ServerService"]}/Name/{name}"); //Old one was PostAsync method 

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync GET to Server_Service was OK");
            }
            else
            {
                Console.WriteLine("--> Sync GET to Server_Service was NOT OK");
            }
        }
    }
}
