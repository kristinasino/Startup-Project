using System;
using Identity.Microservice.Domain.Enums;

namespace Identity.Microservice.Core.Dto.Users
{
    public class UserCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderEnum Gender  { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int? TenantId { get; set; }
        public int[] Locations { get; set; }
    }
}