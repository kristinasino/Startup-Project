using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Tenants;
using Identity.Microservice.Domain.Entities;

namespace Identity.Microservice.Core.Mappings;

public class TenantMapping : Profile
{
    public TenantMapping()
    {
        CreateMap<Tenant, TenantDto>();
        CreateMap<TenantDto, Tenant>();
        CreateMap<TenantCreateDto, Tenant>();
    }
}