using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Dto.Location
{
    public class LocationUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Apt_Suite { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PrimaryPhone { get; set; }
        public string Fname { get; set; }
        public string Middle_Name { get; set; }
        public string Lname { get; set; }
        public int CustomerId { get; set; }
        public int TenantId { get; set; }
    }
}
