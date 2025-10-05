namespace Core.Dto.User
{
    public class UserLoginDto
    {
        public int Id { get; set; }   
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}