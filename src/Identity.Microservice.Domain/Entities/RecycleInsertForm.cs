using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Domain.Entities
{
    public partial class RecycleInsertForm : BaseEntity
    {
        public int CustomerLocationId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool? RecyclingProg { get; set; }
        public bool? Mandated { get; set; }
        public int MaterialTypeId { get; set; }
        public int MethodTypeId { get; set; }
        public decimal? AvgRecyTonMonth { get; set; }
        public decimal? AvgRecyCostMonth { get; set; }
        public decimal? AvgTrashTonsMonth { get; set; }
        public decimal? AvgTrashCostMonth { get; set; }

        public virtual CustomerLocation CustomerLocation { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public virtual MethodType MethodType { get; set; }
    }
}
