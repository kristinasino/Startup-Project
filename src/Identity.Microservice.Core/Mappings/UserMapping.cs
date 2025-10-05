using AutoMapper;
using Core.Dto;
using Core.Dto.User;
using Domain.Entities;
using Identity.Microservice.Core.Dto.FilterPagination;
using Identity.Microservice.Core.Dto.Identity;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Dto.Users;
using Identity.Microservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Core.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.SecurityStamp, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.ToLower().Replace(" ", "")))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => new List<UserRole>()))
                .ForMember(dest => dest.EmailSent, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.UserLocations, opt =>
                {
                    opt.Condition(x => x.Locations is {Length: > 0}); 
                    opt.MapFrom(src => src.Locations.Select(x =>
                        new UserLocation()
                        {
                            LocationId = x
                        }));
                });
            CreateMap<SignUpDto, User>()
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.SecurityStamp, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.ToLower()))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => new List<UserRole>()));
            CreateMap<UserUpdateDto, User>();

            CreateMap<UserRole, UserRoleDto>();
            CreateMap<User, UserLoginDto>();
            CreateMap<PaginateDto<User>, PaginateDto<UserDto>>();
            CreateMap<CustomerLocation, LocationDto>();
            CreateMap<UserLocation, UserLocationsDto>();
        }
    }
}