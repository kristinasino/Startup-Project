using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Identity.Microservice.Domain.Entities;
public class Tenant : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string PhoneNumber { get; set; }
    public string BusinessType { get; set; }
    public string LegalPresentative { get; set; }
}