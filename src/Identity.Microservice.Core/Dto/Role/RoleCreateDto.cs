
namespace Identity.Microservice.Core.Dto.Role
{
    public class RoleCreateDto
    {
        public string Name { get; set; }
        public int[] Permissions { get; set; }
    }
}