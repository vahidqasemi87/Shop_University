using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
