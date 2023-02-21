using AutoMapper;
using Grpc.Core;
using Server_Service.Data;

namespace Server_Service.SyncDataServices.gRPC
{
    public class GrpcCustomerService : GrpcCuctomer.GrpcCuctomerBase
    {
        private readonly ICustomerRepo _repository;
        private readonly IMapper _mapper;

        public GrpcCustomerService(ICustomerRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<SearchReply> GetByName(SearchRequest request, ServerCallContext context)
        {
            var response = new SearchReply();

            var customers = _repository.GetCustomerByName(request.Name);

            foreach (var cust in customers)
            {
                response.Cusotmer.Add(_mapper.Map<GrpcCustomerModel>(cust));
            }

            return Task.FromResult(response);
        }
    }
}
