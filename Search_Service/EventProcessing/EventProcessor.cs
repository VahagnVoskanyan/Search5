using AutoMapper;
using Search_Service.Data;
using Search_Service.Dtos;
using Search_Service.Models;
using System.Text.Json;

namespace Search_Service.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        //Can't call repo in ctor because the lifetime is shorter then in this singleton class
        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CustomerPublished:
                    AddCustomer(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<IEnumerable<GenericEventDto>>(notificationMessage); //list
            //If there is no one with that name
            if (eventType.Count() != 0)
            {
                switch (eventType.First().Event)  //First member
                {
                    case "Customer_Published":
                        Console.WriteLine("--> Customer found 'by name' Event Detected");
                        return EventType.CustomerPublished;
                    default:
                        Console.WriteLine("--> Couldn't determine the event type");
                        return EventType.Undetermined;
                }
            }
            else
            {
                Console.WriteLine("--> NO customer found");
                return EventType.Undetermined;
            }
        }

        private void AddCustomer(string customerPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICustomerRepoS>();

                var customerPublishDto = JsonSerializer.Deserialize<IEnumerable<CustomerPublishDto>>(customerPublishedMessage); //list
                try
                {
                    var cust = _mapper.Map<IEnumerable<Customer>>(customerPublishDto); //list
                    foreach (var item in cust)                       //list
                    {
                        if (!repo.CustomerExists(item.ExternalId))
                        {
                            repo.CreateCustomer(item);
                            repo.SaveChanges();
                            Console.WriteLine("--> Customer added!");
                        }
                        else
                        {
                            Console.WriteLine("--> Customer already exists...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Couldn't add Cusomer to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        CustomerPublished,
        Undetermined
    }
}
