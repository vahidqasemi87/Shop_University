using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class Category
    {
        public Category()
        {
            Subcategory = new HashSet<Subcategory>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string PhotoFilename { get; set; }

        public virtual ICollection<Subcategory> Subcategory { get; set; }
    }
}
