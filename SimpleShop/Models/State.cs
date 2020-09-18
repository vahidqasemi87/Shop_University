using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class State
    {
        public State()
        {
            City = new HashSet<City>();
        }

        public int StateId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
