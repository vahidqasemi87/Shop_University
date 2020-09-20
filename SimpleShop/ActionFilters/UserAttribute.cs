using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleShop.ActionFilters
{
	public class UserAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			//if (!context.HttpContext.Session.Keys.Contains("User"))
			//{
			//    context.HttpContext.Response.Redirect("/home/userlogin");
			//}
			var flag = context.HttpContext.Session.Keys.Contains("User");
			if (!flag)
			{
				context.Result = new RedirectToRouteResult(
					new RouteValueDictionary
					{
						{ "controller","Home"},
						{ "Action","signup"}
					}
					);
				base.OnActionExecuting(context);
			}
		}
	}
}
