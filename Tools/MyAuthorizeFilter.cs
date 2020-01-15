using System.Text;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace NetCoreSample.Tools
{
    public class MyAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public MyAuthorizeFilter() { }
        public bool Authorize([NotNull] DashboardContext context)
        {
            // return context.GetHttpContext().User.Identity.IsAuthenticated;
            return context.GetHttpContext().User.HasClaim(c =>
            {
                return c.Type == "Role" && c.Value == "Admin";
            });
        }
    }
}