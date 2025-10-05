using System;
using Identity.Microservice.Domain.Entities.BaseEntities;

namespace Identity.Microservice.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public int UserId { get; set; }
        
        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }

        public User User { get; set; }
    }
}
