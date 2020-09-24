using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(StateMetaData))]
    public partial class State
    {
 
    }

    public class StateMetaData
    {
        [Required(ErrorMessage ="نام الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="نام")]
        [Remote("IsStateExists", "States", ErrorMessage ="این استان قبلا ثبت شده است")]
        public string Name { get; set; }
    }
}
