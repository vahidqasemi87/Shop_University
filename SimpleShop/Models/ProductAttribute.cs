using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class ProductAttribute
    {
        public ProductAttribute()
        {
            ProductAttributeValue = new HashSet<ProductAttributeValue>();
        }

        public int ProductAttributeId { get; set; }
        public string Name { get; set; }
        public int UnitId { get; set; }

        public virtual Unit Unit { get; set; }
        public virtual ICollection<ProductAttributeValue> ProductAttributeValue { get; set; }
    }
}
