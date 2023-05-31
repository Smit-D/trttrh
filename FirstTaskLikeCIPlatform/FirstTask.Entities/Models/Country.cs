using System;
using System.Collections.Generic;

namespace FirstTask.Entities.Models
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
            States = new HashSet<State>();
            Users = new HashSet<User>();
        }

        public long CountryId { get; set; }
        public string? CountryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<State> States { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
