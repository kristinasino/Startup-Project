namespace Identity.Microservice.Core.Dto
{
    public class TonageInsertFormListDto
    {
        public int Id { get; set; }
        public int CustomerLocationId { get; set; }
        public string CustomerLocationName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal? TrashTonage { get; set; }
        public decimal? CostTrash { get; set; }
        public decimal? RecyclingTonage { get; set; }
        public decimal? RecyclingCost { get; set; }
        public decimal? BarelRental { get; set; }
        public decimal? AvgHaulsPer5CompactedTons { get; set; }
        public decimal? AvgCostHaul { get; set; }
        public decimal? AvgCostDisposal { get; set; }
        public decimal? TotalEstimationIfDisposedAsRegularWaste { get; set; }
        public decimal? ActualCost { get; set; }

    }
}
