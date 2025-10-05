using System.Collections.Generic;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public partial class ContractType : BaseEntity
    {
        public ContractType()
        {
            CustomerContracts = new HashSet<CustomerContract>();
        }

        public string Description { get; set; }

        public virtual ICollection<CustomerContract> CustomerContracts { get; set; }
    }
}
