using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SimpleShop.Models
{
	[ModelMetadataType(typeof(CustomerMetaData))]
	public partial class Customer
	{

	}
	public class CustomerMetaData
	{
		[Required]
		[Display(Name = "شناسه")]
		[Remote("IsExistUserCustomer", "Customers", ErrorMessage = "این نام کاربری قبلا ثبت گردیده است")]
		public string Username { get; set; }
	}
}
