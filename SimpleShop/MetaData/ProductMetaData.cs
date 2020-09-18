using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
        
    }

    public class ProductMetaData
    {
        [Required(ErrorMessage ="نام الزامی است !")]
        [MaxLength(200,ErrorMessage ="حداکثر 200 کاراکتر !")]
        [Display(Name="نام")]
        public string Name { get; set; }
        [Display(Name="زیرگروه")]
        public int SubcategoryId { get; set; }
        [Display(Name="قیمت واحد")]
        public int UnitPrice { get; set; }
        [Display(Name="تصویر اصلی")]
        public string PhotoFilename { get; set; }
    }
}
