using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class City
    {
        public City()
        {
            Customer = new HashSet<Customer>();
        }

        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
    }
}
