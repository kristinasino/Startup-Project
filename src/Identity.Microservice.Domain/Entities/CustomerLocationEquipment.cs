using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities.Identity
{
    public class CustomerLocationEquipment : BaseEntity
    {
        public int CustomerLocationId { get; set; }
        public int ServiceTypeId { get; set; }
        public int EquipmentTypeId { get; set; }
        public string Description { get; set; }
        public string Rental { get; set; }
        public string Haul { get; set; }
        public string Disposal { get; set; }
        public decimal? MonthlyService { get; set; }

        public virtual CustomerLocation CustomerLocation { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }
}
