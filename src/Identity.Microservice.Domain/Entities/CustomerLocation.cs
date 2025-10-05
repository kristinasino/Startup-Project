using System;
using System.Collections.Generic;
using Identity.Microservice.Domain.Entities;
using Identity.Microservice.Domain.Entities.BaseEntities;
using UserModule.Domain.Entities;

namespace Domain.Entities
{
    public partial class CustomerLocation : BaseEntity
    {
        public CustomerLocation()
        {
            CustomerContracts = new HashSet<CustomerContract>();
            ImpdateRecycInsertFrms = new HashSet<RecycleInsertForm>();
            UserLocations = new HashSet<UserLocation>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AptSuite { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PrimaryPhone { get; set; }
        public string Fname { get; set; }
        public string MiddleName { get; set; }
        public string Lname { get; set; }
        public int CustomerId { get; set; }
        public int TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<CustomerContract> CustomerContracts { get; set; }
        public virtual ICollection<RecycleInsertForm> ImpdateRecycInsertFrms { get; set; }
        public virtual ICollection<UserLocation> UserLocations { get; set; }
    }
}
