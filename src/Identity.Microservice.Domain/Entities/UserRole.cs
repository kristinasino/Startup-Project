using Identity.Microservice.Domain.Entities;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace UserModule.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}