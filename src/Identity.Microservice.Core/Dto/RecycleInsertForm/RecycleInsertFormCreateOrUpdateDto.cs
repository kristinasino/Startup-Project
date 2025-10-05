using System.Text.Json.Serialization;

namespace Identity.Microservice.Core.Dto
{
    public partial class RecycleInsertFormCreateOrUpdateDto
    {
        [JsonIgnore]
        public int Id { get; set; }
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
    }
}
