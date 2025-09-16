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

        /// <exception cref="NullReferenceException">If URL is Null</exception>
        public IEnumerable<Customer>? GetCustByName(string name)
        {
            var url = _configuration["GrpcServerS"] ?? throw new NullReferenceException("URL");

            Console.WriteLine($"--> Calling gRPC Service: {url}");

            var channel = GrpcChannel.ForAddress(url);
            var client = new GrpcCustomer.GrpcCustomerClient(channel);
            var request = new SearchRequest { Name = name };

            try
            {
                var reply = client.GetByName(request);
                return _mapper.Map<IEnumerable<Customer>>(reply.Cusotmers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't call gRPC Server {ex.Message}");
                return null; //green line error?
            }
        }
    }
}
