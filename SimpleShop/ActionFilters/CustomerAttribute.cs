using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
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
			var flag = context.HttpContext.Session.Keys.Contains("Customer");
			if (!flag)
			{
				context.Result = new RedirectToRouteResult(
					new RouteValueDictionary
					{
						{ "Controller","Home"},
						{ "Action","signup"}
					}
					);
				base.OnActionExecuting(context);
			}

		}

	}
}
