using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Configurables;

namespace Identity.Microservice.Core.Mappings;

public class ConfigurablesMapping : Profile
{
    public ConfigurablesMapping()
    {
        CreateMap<ServiceType, ServiceTypeDto>();
        CreateMap<EquipmentType, EquipmentTypeDto>();
        CreateMap<Vendor, VendorDto>();
        CreateMap<ContractType, ContractTypeDto>();
    }
}