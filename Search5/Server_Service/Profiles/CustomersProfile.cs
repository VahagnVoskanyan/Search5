using AutoMapper;
using Server_Service.Dtos;
using Server_Service.Models;

namespace Server_Service.Profiles
{
    public class CustomersProfile : Profile //For Dtos dependency injection
    {
        public CustomersProfile()
        {
            // Source -> Target
            CreateMap<Customer, CustomerReadDto>();
             
            CreateMap<CustomerCreateDto, Customer>();

            CreateMap<Customer, CustomerPublishDto>();   //

            CreateMap<CustomerReadDto,CustomerPublishDto>(); //Need?

            CreateMap<Customer, GrpcCustomerModel>();
        }
    }
}
