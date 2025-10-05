using System.Linq;
using AutoMapper;
using Core.Dto;
using Core.Dto.Role;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Dto.Role;
using UserModule.Domain.Entities;

namespace Core.Mappings
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RoleCreateDto, Role>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Replace(" ", "")))
                .ForMember(dest => dest.RolePermissions, opt => opt.MapFrom(src => src.Permissions.Select(x => new RolePermission()
                {
                   PermissionId = x
                })));

            CreateMap<RoleUpdateDto, Role>();
            
            CreateMap<RolePermission, RolePermissionDto>();
            CreateMap<Permission, PermissionDto>();
        }
    }
}