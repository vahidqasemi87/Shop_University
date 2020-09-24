using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(SubcategoryMetaData))]
    public partial class Subcategory
    {
        
    }

    public class SubcategoryMetaData
    {
        [Required(ErrorMessage ="نام الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="نام")]
        [Remote("IsSubCategoryNameExists", "Subcategories", ErrorMessage ="این نام قبلا ثبت گردیده است")]
        public string Name { get; set; }
        [Display(Name="گروه")]
        public int CategoryId { get; set; }
    }
}
