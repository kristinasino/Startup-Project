using System;
using System.Collections.Generic;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public partial class MaterialType : BaseEntity
    {
        public MaterialType()
        {
            ImpdateRecycInsertFrms = new HashSet<RecycleInsertForm>();
        }

        public string Description { get; set; }

        public virtual ICollection<RecycleInsertForm> ImpdateRecycInsertFrms { get; set; }
    }
}
