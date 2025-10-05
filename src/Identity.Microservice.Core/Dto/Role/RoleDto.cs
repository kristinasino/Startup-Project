using System.Collections;
using System.Collections.Generic;

namespace Core.Dto.Role
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RolePermissionDto> RolePermissions { get; set; }
    }
}