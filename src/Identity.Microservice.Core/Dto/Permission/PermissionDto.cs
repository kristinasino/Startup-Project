namespace Core.Dto
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
    }
}