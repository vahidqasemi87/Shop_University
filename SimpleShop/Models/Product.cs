using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class Product
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
            ProductAttributeValue = new HashSet<ProductAttributeValue>();
            ProductPhoto = new HashSet<ProductPhoto>();
        }

        public int ProductId { get; set; }
        public int SubcategoryId { get; set; }
        public string Name { get; set; }
        public int UnitPrice { get; set; }
        public string PhotoFilename { get; set; }

        public virtual Subcategory Subcategory { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual ICollection<ProductAttributeValue> ProductAttributeValue { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhoto { get; set; }
    }
}
