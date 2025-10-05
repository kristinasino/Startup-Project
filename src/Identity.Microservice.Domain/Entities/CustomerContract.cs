using System;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public class CustomerContract : BaseEntity
    {
        public int CustomerLocationId { get; set; }
        public int ContractTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VendorId { get; set; }


        public virtual ContractType ContractType { get; set; }
        public virtual CustomerLocation CustomerLocation { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
