using System;
using System.Collections.Generic;

namespace FirstTask.Entities.Models
{
    public partial class City
    {
        public City()
        {
            Users = new HashSet<User>();
        }

        public long CityId { get; set; }
        public string? CityName { get; set; }
        public long? StateId { get; set; }
        public long? CountryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Country? Country { get; set; }
        public virtual State? State { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
