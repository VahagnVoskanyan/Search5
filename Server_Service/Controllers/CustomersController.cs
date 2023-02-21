using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server_Service.AsyncDataServices;
using Server_Service.Data;
using Server_Service.Dtos;
using Server_Service.Models;

namespace Server_Service.Controllers
{
    [Route("api/[controller]")] //==api/Customers
    [ApiController]
    public class CustomersController : Controller //InVideo this is ControllerBase 
    {
        private readonly ICustomerRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public CustomersController(
            ICustomerRepo repository,
            IMapper mapper,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet] //Swagger doesn't work without this ))
        public ActionResult<IEnumerable<CustomerReadDto>> GetCustomers()
        {
            Console.WriteLine("--> Getting Customers...");

            var custs = _repository.GetAllCustomers(); //this is Customer type 

            return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(custs));
        }
        [HttpGet("{id}", Name = "Get By Id")]
        public ActionResult<CustomerReadDto> GetById(int id)
        {
            Console.WriteLine("--> Getting Customer By id...");

            var cust = _repository.GetCustomerById(id);
            if (cust != null)
            {
                return Ok(_mapper.Map<CustomerReadDto>(cust));
            }
            return NotFound();
        }
        [HttpGet("Name/{name}")]
        public ActionResult<IEnumerable<CustomerReadDto>> GetByName(string name)
        {
            Console.WriteLine("--> Getting Customer By Name...");

            var cust = _repository.GetCustomerByName(name);
            var readDto = _mapper.Map<IEnumerable<CustomerReadDto>>(cust);

            if (cust != null)
            {
                //Send Async Message
                try
                {
                    var customerPublishDto = _mapper.Map<IEnumerable<CustomerPublishDto>> (readDto);
                    foreach (var item in customerPublishDto)
                    {
                        item.Event = "Customer_Published";
                    }
                    //customerPublishDto.Event = "Customer_Published";
                    _messageBusClient.PublishCustomerByName(customerPublishDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Couldn't send asynchronously: {ex.Message}");
                }

                return Ok(readDto);
            }
            return NotFound();
        }
        [HttpPost]
        public ActionResult<CustomerReadDto> Create(CustomerCreateDto customerCreateDto) 
        {
            var customerModel = _mapper.Map<Customer>(customerCreateDto);
            _repository.CreateCustomer(customerModel);
            _repository.SaveChanges();

            var customerReadDto = _mapper.Map<CustomerReadDto>(customerModel);

            return CreatedAtRoute("Get By Id", new { Id = customerReadDto.Id }, customerReadDto);  //Why new Id ??
        }
        [HttpPost("test")]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Server Service");

            return Ok("Inbound test of from customers controller");
        }
    }
}
