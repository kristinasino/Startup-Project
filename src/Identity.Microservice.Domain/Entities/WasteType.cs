using System.Collections.Generic;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public partial class WasteType : BaseEntity
    {
        public WasteType()
        {
            EquipmentTypes = new HashSet<EquipmentType>();
        }

        public string Description { get; set; }

        public virtual ICollection<EquipmentType> EquipmentTypes { get; set; }
    }
}
