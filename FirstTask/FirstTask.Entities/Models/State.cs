using System;
using System.Collections.Generic;

namespace FirstTask.Entities.Models
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<City>();
            Users = new HashSet<User>();
        }

        public long StateId { get; set; }
        public string? StateName { get; set; }
        public long? CountryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Country? Country { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
