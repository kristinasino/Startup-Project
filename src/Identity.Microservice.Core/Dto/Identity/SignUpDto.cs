using Identity.Microservice.Domain.Enums;

namespace Identity.Microservice.Core.Dto.Identity
{
    public class SignUpDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public GenderEnum Gender  { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}