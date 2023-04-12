using AutoMapper;
using Server_Service.Data;
using Server_Service.Dtos;
using Server_Service.Models;

namespace Search_Service.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        //Can't call repo in MessageBusSubscriber because the lifetime is shorter then in this singleton class
        public EventProcessor(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public IEnumerable<CustomerPublishDto> ProcessEvent(string message)
        {
            var custs = FindCustomers(message);

            //Send Async Message

            var customerPublishDto = _mapper.Map<IEnumerable<CustomerPublishDto>>(custs);
            foreach (var item in customerPublishDto)
            {
                item.Event = "Customer_Published";
            }

            return customerPublishDto;
        }

        private IEnumerable<Customer> FindCustomers(string name)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICustomerRepo>();

                var custs = repo.GetCustomerByName1(name);   //

                return custs;
            }
        }
    }
}
