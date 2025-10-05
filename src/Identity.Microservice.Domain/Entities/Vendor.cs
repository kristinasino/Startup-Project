using System.Collections.Generic;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public partial class Vendor : BaseEntity
    {
        public Vendor()
        {
            CustomerContracts = new HashSet<CustomerContract>();
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Apt_Suite { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PrimaryContactPerson { get; set; }

        public virtual ICollection<CustomerContract> CustomerContracts { get; set; }
    }
}
