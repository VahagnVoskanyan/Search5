using AutoMapper;
using Server_Service.EventProcessing;
using Server_Service.Data;
using Server_Service.Dtos;
using Server_Service.Models;

namespace Server_Service.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        //Can't call repo in MessageBusSubscriber because the lifetime is shorter then in this singleton class
        public EventProcessor(IMapper mapper,
            IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public IEnumerable<CustomerPublishDto> ProcessEvent(string message)
        {
            var custs = FindCustomers(message);

            //Send Async Message
            var customerPublishDto = _mapper.Map<IEnumerable<CustomerPublishDto>>(custs);
            foreach (var item in customerPublishDto)
            {
                item.Event = EventType.Customer_Published.ToString();
            }

            return customerPublishDto;
        }

        private IEnumerable<Customer> FindCustomers(string name)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICustomerRepo>();

                var custs = repo.GetByNameSim(name);   //

                return custs;
            }
        }
    }

    enum EventType
    {
        Customer_Published,
        Undetermined
    }
}
