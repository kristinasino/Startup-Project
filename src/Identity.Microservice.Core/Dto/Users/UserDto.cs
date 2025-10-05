using System;
using System.Collections.Generic;
using Core.Dto;
using Identity.Microservice.Domain.Enums;

namespace Identity.Microservice.Core.Dto.Users
{
    public class UserDto
    {
        public int Id { get; set; }   
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderEnum Gender { get; set; }
        public int? TenantId { get; set; }
        public IEnumerable<UserRoleDto> UserRoles { get; set; }
        public IEnumerable<UserLocationsDto> UserLocations { get; set; }
    }
}