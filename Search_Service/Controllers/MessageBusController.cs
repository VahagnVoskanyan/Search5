using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Search_Service.AsyncDataServices;
using Search_Service.Dtos;

namespace Search_Service.Controllers
{
    [Route("api/s/[controller]")]
    [ApiController]
    public class MessageBusController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;

        public MessageBusController(IMessageBusClient messageBusClient,
            IMapper mapper)
        {
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }

        // Server Service
        [HttpPost("Create", Name = "Send new Consumer to Bus")]
        public IActionResult SendName([FromBody] CustomerCreateDto customerCreateDto)
        {
            var customerPublishDto = _mapper.Map<CustomerPublishDto>(customerCreateDto);
            customerPublishDto.Event = "Customer_Published";

            //Send Async Message
            try
            {
                _messageBusClient.SendNewCustomer(customerPublishDto);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't send or receive asynchronously: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
