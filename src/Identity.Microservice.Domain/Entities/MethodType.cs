using System.Collections.Generic;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public partial class MethodType : BaseEntity
    {
        public MethodType()
        {
            ImpdateRecycInsertFrms = new HashSet<RecycleInsertForm>();
        }

        public string Description { get; set; }

        public virtual ICollection<RecycleInsertForm> ImpdateRecycInsertFrms { get; set; }
    }
}
