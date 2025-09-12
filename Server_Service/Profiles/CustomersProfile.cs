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

            CreateMap<Customer, CustomerPublishedDto>();   //

            CreateMap<CustomerReadDto,CustomerPublishedDto>(); //Need?

            CreateMap<CustomerPublishedDto, Customer>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => default(int)));

            CreateMap<Customer, GrpcCustomerModel>();
        }
    }
}
