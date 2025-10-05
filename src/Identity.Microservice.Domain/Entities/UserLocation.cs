using Domain.Entities;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace UserModule.Domain.Entities;

public class UserLocation : BaseEntity
{
    public int UserId { get; set; }
    public int LocationId { get; set; }
    public CustomerLocation Location { get; set; }
}