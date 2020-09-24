using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string IsAdminString
        {
            get
            {
                return IsAdmin ? "بله" : "خیر";
            }
        }
    }

    public class UserMetaData
    {
        [Required(ErrorMessage ="شناسه الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="شناسه")]
        [Remote("IsUserExists", "Users", ErrorMessage ="این نام کاربری قبلا ثبت گردیده است")]
        public string Username { get; set; }
        [Required(ErrorMessage ="رمز الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="رمز")]
        public string Password { get; set; }
        [Required(ErrorMessage ="نام خانوادگی الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="نام خانوادگی")]
        public string Family { get; set; }
        [Required(ErrorMessage ="نام الزامی است !")]
        [MaxLength(50,ErrorMessage ="حداکثر 50 کاراکتر !")]
        [Display(Name="نام")]
        public string Name { get; set; }
        [Required(ErrorMessage ="موبایل الزامی است !")]
        [MaxLength(11,ErrorMessage ="حداکثر 11 کاراکتر !")]
        [Display(Name="موبایل")]
        public string Mobile { get; set; }
        [Display(Name="مدیر")]
        public string IsAdmin { get; set; }
    }
}
