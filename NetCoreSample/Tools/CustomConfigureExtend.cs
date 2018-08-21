using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Models;

namespace NetCoreSample.Tools
{
    public static class CustomConfigureExtend
    {
        public static IServiceCollection AddCustomConfigureExtend(this IServiceCollection services)
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