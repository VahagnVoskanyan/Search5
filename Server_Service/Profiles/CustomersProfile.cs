using AutoMapper;
using Server_Service.Dtos;
using Server_Service.Models;

namespace Server_Service.Profiles
{
    public class CustomersProfile : Profile //For Dtos dependency injection
    {
        public CustomersProfile()
        {
            //Source(model) -> Target(dto)
            CreateMap<Customer, CustomerReadDto>();
            //Source(dto) -> target(model).  
            CreateMap<CustomerCreateDto, Customer>();

            CreateMap<CustomerReadDto,CustomerPublishDto>();

            CreateMap<Customer, GrpcCustomerModel>();
        }
    }
}
