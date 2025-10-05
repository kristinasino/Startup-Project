using AutoMapper;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Dto.CustomerEquipment;

namespace Identity.Microservice.Core.Mappings;

public class CustomerEquipmentMapping : Profile
{
    public CustomerEquipmentMapping()
    {
        CreateMap<CustomerLocationEquipment, CustomerEquipmentDto>()
            .ForMember(dest => dest.CustomerLocationName, opt
                => opt.MapFrom(src => src.CustomerLocation.Name))
            .ForMember(dest => dest.EquipmentTenantId, opt
                => opt.MapFrom(src => src.CustomerLocation.TenantId))
            .ForMember(dest => dest.ServiceTypeName, opt
                => opt.MapFrom(src => src.ServiceType.Description))
            .ForMember(dest => dest.EquipmentTypeName, opt => opt.MapFrom(src => src.EquipmentType.Description));
        CreateMap<CustomerEquipmentCreateDto, CustomerLocationEquipment>();
        CreateMap<CustomerEquipmentUpdateDto, CustomerLocationEquipment>();
    }
}