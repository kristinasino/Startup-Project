using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Location;

namespace Identity.Microservice.Core.Mappings;

public class LocationMapping : Profile
{
    public LocationMapping()
    {
        CreateMap<CustomerLocation, LocationDto>();
        CreateMap<LocationCreateDto, CustomerLocation >();
        CreateMap<LocationUpdateDto, CustomerLocation >();
    }
}