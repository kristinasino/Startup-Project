namespace Identity.Microservice.Core.Dto.CustomerEquipment;

public class CustomerEquipmentUpdateDto
{
    public int Id { get; set; }
    public int CustomerLocationId { get; set; }
    public int ServiceTypeId { get; set; }
    public int EquipmentTypeId { get; set; }
    public string Description { get; set; }
    public string Rental { get; set; }
    public string Haul { get; set; }
    public string Disposal { get; set; }
    public decimal? MonthlyService { get; set; }
}