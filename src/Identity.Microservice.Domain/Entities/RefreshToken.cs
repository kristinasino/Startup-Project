using System;

namespace Domain.Entities.Identity
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }
        public int UserId { get; set; }
    }
}