using System;

namespace Identity.Microservice.Core.Dto.Users
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int RoleId { get; set; }
        
        public int? TenantId { get; set; }
        public int[] Locations { get; set; }
    }
}