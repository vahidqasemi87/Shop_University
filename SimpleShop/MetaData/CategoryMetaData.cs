using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {
 
    }

    public class CategoryMetaData
    {
        [Required(ErrorMessage ="نام الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="نام")]
        [Remote("IsCategoryNameExists", "Categories", ErrorMessage ="این نام تکراری می باشد")]
        public string Name { get; set; }
    }
}
