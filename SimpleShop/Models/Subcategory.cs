using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class Subcategory
    {
        public Subcategory()
        {
            Product = new HashSet<Product>();
        }

        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string PhotoFilename { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
