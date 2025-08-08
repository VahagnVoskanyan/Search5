using AutoMapper;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Search_Service.Models;
using Search_Service.Protos;

namespace Search_Service.SyncDataServices.gRPC
{
    public class GrpcDataClient : IGrpcDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GrpcDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public IEnumerable<Customer> GetCustByName(string name)
        {
            Console.WriteLine($"--> Calling gRPC Service: {_configuration["GrpcServerS"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcServerS"]);
            var client = new GrpcCuctomer.GrpcCuctomerClient(channel);
            var request = new SearchRequest { Name = name };

            try
            {
                var reply = client.GetByName(request);
                return _mapper.Map<IEnumerable<Customer>>(reply.Cusotmer); //reply.Customer ??
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't call gRPC Server {ex.Message}");
                return null; //green line error?
            }
        }
    }
}
