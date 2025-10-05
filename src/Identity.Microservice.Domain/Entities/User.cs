using System;
using System.Collections.Generic;
using Domain.Entities;
using Identity.Microservice.Domain.Entities.BaseEntities;
using Identity.Microservice.Domain.Enums;
using UserModule.Domain.Entities;

namespace Identity.Microservice.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderEnum Gender { get; set; }

        public int? TenantId { get; set; }
        public bool EmailSent { get; set; }

        public Tenant Tenant { get; set; }
        public ICollection<UserLocation> UserLocations { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}