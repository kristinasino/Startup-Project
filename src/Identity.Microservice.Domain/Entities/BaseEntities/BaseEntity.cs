using System;

namespace Identity.Microservice.Domain.Entities.BaseEntities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsSoftDeleted { get; set; }
        public int Version { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}