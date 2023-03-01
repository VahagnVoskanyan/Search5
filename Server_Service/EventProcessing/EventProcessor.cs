using AutoMapper;
using Google.Protobuf.Collections;
using Server_Service.AsyncDataServices;
using Server_Service.Data;
using Server_Service.Dtos;
using Server_Service.Models;
using System.Text.Json;

namespace Search_Service.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;

        //Can't call repo in MessageBusSubscriber because the lifetime is shorter then in this singleton class
        public EventProcessor(
            IServiceScopeFactory scopeFactory,
            //IMessageBusClient messageBusClient,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            //_messageBusClient = messageBusClient;
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

            //Without RPC
            /*try
            {
                var customerPublishDto = _mapper.Map<IEnumerable<CustomerPublishDto>>(custs);
                foreach (var item in customerPublishDto)
                {
                    item.Event = "Customer_Published";
                }

                return customerPublishDto;

                _messageBusClient.PublishCustomerByName(customerPublishDto);      //Sending to Message Bus
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't send asynchronously: {ex.Message}");
            }*/
        }

        private IEnumerable<Customer> FindCustomers(string name)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICustomerRepo>();

                var custs = repo.GetCustomerByName(name);

                return custs; //if it is null?
            }
        }
    }
}
