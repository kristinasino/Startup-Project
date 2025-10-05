using Identity.Microservice.Core.Dto.Location;

namespace Identity.Microservice.Core.Dto.Users;

public class UserLocationsDto
{
    public int LocationId { get; set; }
    public int UserId { get; set; }

    public LocationDto Location { get; set; }
}