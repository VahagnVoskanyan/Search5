using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Search_Service.AsyncDataServices;
using Search_Service.Dtos;
using System.Text.Json;

namespace Search_Service.Controllers
{
    [Route("api/s/customers/[controller]")]
    [ApiController]
    public class RpcController : ControllerBase
    {
        private readonly IRpcBusClient _rpcBusClient;
        private readonly IMapper _mapper;

        public RpcController(IRpcBusClient rpcBusClient,
            IMapper mapper)
        {
            _rpcBusClient = rpcBusClient;
            _mapper = mapper;
        }

        // Server Service
        [HttpGet("Name", Name = "Send Name To Bus")]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> SendName([FromQuery] string name)
        {
            //Send Async Message RPC
            try
            {
                var response = await _rpcBusClient.SendNameToBusAsync(name);

                //We can do this without Mapper (deserializing to CustomerReadDto)
                var customerPublishDto = JsonSerializer.Deserialize<IEnumerable<CustomerPublishedDto>>(response);
                var customerReadDto = _mapper.Map<IEnumerable<CustomerReadDto>>(customerPublishDto);

                return Ok(customerReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't send or receive asynchronously: {ex.Message}");
                return StatusCode(503);
            }
        }
    }
}
