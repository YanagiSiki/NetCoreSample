using System.Text;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace NetCoreSample.Tools
{
    public class MyAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public MyAuthorizeFilter() { }
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var isAuthorized = httpContext.User.HasClaim(c =>
            {
                return c.Type == "Role" && c.Value == "Admin";
            });
            if (!isAuthorized)
            {
                httpContext.Response.Redirect("/Home/Login");
            }
            return true;
        }
    }
}