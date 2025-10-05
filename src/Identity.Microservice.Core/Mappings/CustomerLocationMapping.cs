using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.CustomerLocation;

namespace Identity.Microservice.Core.Mappings;

public class CustomerLocationMapping : Profile
{
    public CustomerLocationMapping()
    {
        CreateMap<CustomerLocation, CustomerLocationDto>();

    }
}