﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace NetCoreSample.Authorize
{
    /// <summary>
    /// 未登入才可看見
    /// </summary>
    public class IsNotLoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "index" })
                );

                context.Result.ExecuteResultAsync(context);
            }
            else base.OnActionExecuting(context);
        }
    }
}