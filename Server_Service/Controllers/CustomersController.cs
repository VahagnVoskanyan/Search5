using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server_Service.Data;
using Server_Service.Dtos;
using Server_Service.Models;

namespace Server_Service.Controllers
{
    [Route("api/[controller]")] //==api/Customers
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _custRepo;
        private readonly IMapper _mapper;

        public CustomersController(
            ICustomerRepo repository,
            IMapper mapper)
        {
            _custRepo = repository;
            _mapper = mapper;
        }

        [HttpGet] //Swagger doesn't work without this ))
        public ActionResult<IEnumerable<CustomerReadDto>> GetAllCusts()
        {
            Console.WriteLine("--> Getting Customers...");

            var custs = _custRepo.GetAll();

            return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(custs));
        }

        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<CustomerReadDto> GetCustById(int id)
        {
            Console.WriteLine("--> Getting Customer By id...");

            var cust = _custRepo.GetById(id);
            if (cust != null)
            {
                return Ok(_mapper.Map<CustomerReadDto>(cust));
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<CustomerReadDto> CreateCust(CustomerCreateDto customerCreateDto)
        {
            var customerModel = _mapper.Map<Customer>(customerCreateDto);
            _custRepo.Create(customerModel);
            _custRepo.SaveChanges();

            var customerReadDto = _mapper.Map<CustomerReadDto>(customerModel);

            return CreatedAtRoute(nameof(GetCustById), new { customerReadDto.Id }, customerReadDto);
        }


        [HttpGet("Name/{name}", Name = "Get By Name")]
        public ActionResult<IEnumerable<CustomerReadDto>> GetCustByName(string name)
        {
            Console.WriteLine("--> Getting Customer By name...");

            var cust = _custRepo.GetByNameSim(name);
            if (cust != null)
            {
                return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(cust));
            }
            return NotFound();
        }
    }
}
