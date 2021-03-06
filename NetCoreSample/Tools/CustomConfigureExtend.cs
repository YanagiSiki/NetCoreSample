using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Models;

namespace NetCoreSample.Tools
{
    public static class CustomConfigureExtend
    {
        public static IServiceCollection AddCustomAuthExtend(this IServiceCollection services)
        {
            services.AddAuthentication("CookieForView")
                .AddCookie("CookieForView", options =>
                {
                    options.AccessDeniedPath = new PathString("/Home/Error");
                    options.LoginPath = new PathString("/Home/Login");
                    options.LogoutPath = new PathString("/Home/Logout");
                });
            services.AddAuthentication("CookieForWebApi")
                .AddCookie("CookieForWebApi", options =>
                {
                    // https://stackoverflow.com/questions/32863080/how-to-remove-the-redirect-from-an-asp-net-core-webapi-and-return-http-401
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                            {
                                context.Response.StatusCode = 401;
                                return Task.CompletedTask;;
                            },
                            OnRedirectToAccessDenied = context =>
                            {
                                context.Response.StatusCode = 403;
                                return Task.CompletedTask;;
                            }
                    };
                });
            return services;
        }

        public static IServiceCollection AddCustomPolicyExtend(this IServiceCollection services)
        {
            //*** Admin ***
            services.AddAuthorization(options =>
            {
                //*** Admin ***
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(c =>
                        {
                            return c.Type == Roles.Role && c.Value == Roles.Admin;
                        });
                    });
                });
            });
            return services;
        }
    }

}