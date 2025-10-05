
namespace Domain.Entities.Identity
{
    public class Permission
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
    }
}