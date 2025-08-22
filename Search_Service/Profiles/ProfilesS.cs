using AutoMapper;
using Search_Service.Dtos;
using Search_Service.Models;
using Search_Service.Protos;

namespace Search_Service.Profiles
{
    public class ProfilesS : Profile
    {
        public ProfilesS()
        {
            // Source -> Target
            CreateMap<CustomerPublishDto, Customer>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            
            CreateMap<CustomerPublishDto, CustomerReadDto>(); //??
            
            CreateMap<Customer, CustomerReadDto>();

            CreateMap<GrpcCustomerModel, Customer>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<CustomerCreateDto, CustomerPublishDto>();
        }
    }
}
