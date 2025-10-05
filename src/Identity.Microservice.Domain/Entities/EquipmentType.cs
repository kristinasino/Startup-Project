using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public partial class EquipmentType : BaseEntity
    {
        public string Description { get; set; }
        public int WasteTypeId { get; set; }

        public virtual WasteType WasteType { get; set; }
    }
}
