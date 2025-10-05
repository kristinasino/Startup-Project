using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Contract;

namespace Identity.Microservice.Core.Mappings
{
    public class ContractMapping: Profile
    {
        public ContractMapping()
        {
            CreateMap<CustomerContract, ContractDto>()
                .ForMember(dest => dest.ContractTypeName , opt => opt.MapFrom(src => src.ContractType.Description))
                .ForMember(dest => dest.CustomerLocationName , opt => opt.MapFrom(src => src.CustomerLocation.Name))
                .ForMember(dest => dest.CustomerLocationTenantId , opt => opt.MapFrom(src => src.CustomerLocation.TenantId))
                .ForMember(dest => dest.VendorName , opt => opt.MapFrom(src => src.Vendor.Code));
            CreateMap<ContractCreateDto, CustomerContract>();
            CreateMap<ContractUpdateDto, CustomerContract>();
        }
    }
}
