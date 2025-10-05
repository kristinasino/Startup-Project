using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Dto.Contract
{
    public class ContractDto
    {
        public int Id { get; set; }
        public int CustomerLocationId { get; set; }
        public string CustomerLocationName { get; set; }
        public int CustomerLocationTenantId { get; set; }
        public int ContractTypeId { get; set; }
        public string ContractTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
    }
}
