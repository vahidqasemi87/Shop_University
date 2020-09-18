using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(CityMetaData))]
    public partial class City
    {
        
    }

    public class CityMetaData
    {
        [Required(ErrorMessage ="نام الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="نام")]
        public string Name { get; set; }
        [Display(Name="استان")]
        public int StateId { get; set; }
    }
}
