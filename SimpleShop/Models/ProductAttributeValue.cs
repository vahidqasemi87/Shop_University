using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class ProductAttributeValue
    {
        public int ProductAttributeValueId { get; set; }
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public string Value { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductAttribute ProductAttribute { get; set; }
    }
}
