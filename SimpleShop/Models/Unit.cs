using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class Unit
    {
        public Unit()
        {
            ProductAttribute = new HashSet<ProductAttribute>();
        }

        public int UnitId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductAttribute> ProductAttribute { get; set; }
    }
}
