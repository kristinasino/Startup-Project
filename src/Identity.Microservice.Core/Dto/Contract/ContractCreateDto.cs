using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Dto.Contract
{
    public class ContractCreateDto
    {
        public int CustomerLocationId { get; set; }
        public int ContractTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VendorId { get; set; }
    }
}
