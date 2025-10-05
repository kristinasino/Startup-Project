using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Common;
using Identity.Microservice.Core.Dto.Lookup;

namespace Identity.Microservice.Core.Mappings;

public class LookupMapping : Profile
{
    public LookupMapping()
    {
        CreateMap<WasteType, LookupBaseDto>();
        CreateMap<MaterialType, LookupBaseDto>();
        CreateMap<MethodType, LookupBaseDto>();
        CreateMap<ServiceType, LookupBaseDto>();
        CreateMap<WasteType, LookupBaseDto>();
        CreateMap<EquipmentType, EquipmentTypeDto>();
    }
}