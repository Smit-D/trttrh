using System;
using System.Collections.Generic;

namespace FirstTask.Entities.Models
{
    public partial class User
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Avtar { get; set; }
        public string? PhoneNumber { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public byte? GenderId { get; set; }
        public byte? RoleId { get; set; }

        public virtual City? City { get; set; }
        public virtual Country? Country { get; set; }
        public virtual Gender? Gender { get; set; }
        public virtual Role? Role { get; set; }
        public virtual State? State { get; set; }
    }
}
