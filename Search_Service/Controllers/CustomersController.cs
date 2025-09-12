using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Search_Service.Data;
using Search_Service.Dtos;
using Search_Service.SyncDataServices.gRPC;

namespace Search_Service.Controllers
{
    [Route("api/s/customers")] //If the name is the same as the previous one
                                  //there will be problems with API Gateway
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IGrpcDataClient _grpcDataClient;
        private readonly ICustomerRepoS _repository;
        private readonly IMapper _mapper;

        public CustomersController(
            IGrpcDataClient grpcDataClient,
            ICustomerRepoS repository,
            IMapper mapper)
        {
            _grpcDataClient = grpcDataClient;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("gRPC/{name}", Name = "Send Name by gRPC")]
        public ActionResult<IEnumerable<CustomerReadDto>> SendNameGrpc(string name)
        {
            Console.WriteLine("--> Sending Name by gRPC");

            try
            {
                var custs = _grpcDataClient.GetCustByName(name);
                /*foreach (var item in custs)                         //To Save in DB
                {
                    if (!_repository.CustomerExists(item.ExternalId))
                    {
                        _repository.CreateCustomer(item);
                        _repository.SaveChanges();
                        Console.WriteLine("--> Customer added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Customer already exists...");
                    }
                }*/
                return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(custs));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't send synchronously: {ex.Message}");
                return BadRequest(ex.Message); //
            }
        }

        //Search Service
        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadDto>> GetCustomers()
        {
            Console.WriteLine("--> Getting Customers...");

            var custs = _repository.GetAllCustomers();

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
    }
}
