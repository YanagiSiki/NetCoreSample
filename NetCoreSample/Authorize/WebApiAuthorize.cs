using System;
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
    public class WebApiAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var httpContext = context.HttpContext;
            var isAuthorized = context.HttpContext.User.HasClaim(c =>
            {
                return c.Type == "Role" && c.Value == "Admin";
            });
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }

            else base.OnActionExecuting(context);
        }
    }
}