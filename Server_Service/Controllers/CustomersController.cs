using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public CustomersController(
            ICustomerRepo repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        [HttpPost]
        public ActionResult<CustomerReadDto> Create(CustomerCreateDto customerCreateDto)
        {
            var customerModel = _mapper.Map<Customer>(customerCreateDto);
            _repository.CreateCustomer(customerModel);
            _repository.SaveChanges();

            var customerReadDto = _mapper.Map<CustomerReadDto>(customerModel);

            return CreatedAtRoute("Get By Id", new { Id = customerReadDto.Id }, customerReadDto);  //Why new Id ??
        }
    }
}
