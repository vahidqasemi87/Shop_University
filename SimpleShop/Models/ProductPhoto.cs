using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class ProductPhoto
    {
        public int ProductPhotoId { get; set; }
        public int ProductId { get; set; }
        public string PhotoFilename { get; set; }

        public virtual Product Product { get; set; }
    }
}
