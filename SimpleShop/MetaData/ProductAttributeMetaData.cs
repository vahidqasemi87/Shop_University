using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(ProductAttributeMetaData))]
    public partial class ProductAttribute
    {
        
    }

    public class ProductAttributeMetaData
    {
        [Required(ErrorMessage ="نام الزامی است !")]
        [MaxLength(100,ErrorMessage ="حداکثر 100 کاراکتر !")]
        [Display(Name="نام")]
        public string Name { get; set; }
        [Display(Name="واحد اندازه گیری")]
        public int UnitId { get; set; }
    }
}
