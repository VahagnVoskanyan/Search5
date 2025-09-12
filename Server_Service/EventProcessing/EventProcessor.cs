using AutoMapper;
using Google.Protobuf.Collections;
using Server_Service.Data;
using Server_Service.Dtos;
using Server_Service.EventProcessing;
using Server_Service.Models;
using System.Text.Json;

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

        public IEnumerable<CustomerPublishedDto> ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.SearchByName:
                    var customers = FindCustomers(message);
                    return MakeCustPublishDtos(customers);
                case EventType.Customer_Published:
                    CreateCustomer(message);
                    return [];
                default:
                    Console.WriteLine("--> Event not recognized");
                    return [];
            }
        }

        private IEnumerable<Customer> FindCustomers(string message)
        {
            var custSearchDto = JsonSerializer.Deserialize<CustomerSearchDto>(message);

            if (custSearchDto == null)
            {
                Console.WriteLine("--> CustomerSearchDto is NULL");
                return [];
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICustomerRepo>();

                var custs = repo.GetByNameSim(custSearchDto.Name);   //

                return custs;
            }
        }

        private IEnumerable<CustomerPublishedDto> MakeCustPublishDtos(IEnumerable<Customer> customers)
        {
            // Make Async response Message
            var customerPublishDto = _mapper.Map<IEnumerable<CustomerPublishedDto>>(customers);
            foreach (var item in customerPublishDto)
            {
                item.Event = EventType.Customer_Published.ToString();
            }

            return customerPublishDto;
        }

        private void CreateCustomer(string message)
        {
            var custPublishedDto = JsonSerializer.Deserialize<CustomerPublishedDto>(message);

            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICustomerRepo>();

                try
                {
                    var cust = _mapper.Map<Customer>(custPublishedDto);

                    if (!repo.ExternalIdExists(cust.ExternalId))
                    {
                        repo.Create(cust);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Customer added! {cust.ExternalId} - {cust.Name} {cust.Surname}");
                    }
                    else
                    {
                        Console.WriteLine($"--> Customer already exists... {cust.ExternalId} - {cust.Name} {cust.Surname}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Couldn't add Customer to DB {ex.Message}");
                }
            }
        }

        private static EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            if (eventType == null)
            {
                Console.WriteLine("No event Type");
                return EventType.Undetermined;
            }

            switch (eventType.Event)
            {
                case "SearchByName":
                    Console.WriteLine("--> SearchByName Event detected");
                    return EventType.SearchByName;
                case "Customer_Published":
                    Console.WriteLine("--> Customer Published Event detected");
                    return EventType.Customer_Published;
                default:
                    Console.WriteLine("--> Couldn't determine the Event Type");
                    return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        Customer_Published,
        SearchByName,
        Undetermined
    }
}
