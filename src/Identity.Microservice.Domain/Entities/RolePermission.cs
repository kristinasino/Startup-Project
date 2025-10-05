using Domain.Entities.Identity;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace UserModule.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}