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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = new PathString("/Home/Error");
                    options.LoginPath = new PathString("/Home/Login");
                    options.LogoutPath = new PathString("/Home/Logout");
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