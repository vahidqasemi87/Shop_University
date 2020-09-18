using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.ActionFilters
{
	public class CustomerAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.HttpContext.Session.Keys.Contains("Customer"))
			{
				context.HttpContext.Response.Redirect("/home/signup");
			}
		}
	}
}
