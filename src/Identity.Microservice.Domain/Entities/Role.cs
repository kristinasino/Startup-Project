using System;
using System.Collections.Generic;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace UserModule.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}