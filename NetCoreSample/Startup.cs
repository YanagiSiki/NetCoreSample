using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NetCoreSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("BadgeEntry",
            //        policy => policy.RequireAssertion(context =>
            //                context.User.HasClaim(c =>
            //                    (c.Type == "Admin" ||
            //                    c.Type == "User")
            //                    && c.Issuer == "https://microsoftsecurity"));
            //}));
            services.AddAuthentication("UserLog")
            .AddCookie("UserLog", options => {
                options.AccessDeniedPath = "/Error";
                options.LoginPath = "/Login";
            });
            services.AddAuthorization(options => {
                //options.AddPolicy("NotLogin", policy => {
                //    policy.RequireClaim(Roles.Role, Roles.Admin);
                //});
                //*** NotLogin ***
                options.AddPolicy("NotLogin", policy => {
                    policy.RequireAssertion(context => {
                        return !context.User.HasClaim(c => {
                            return c.Type == Roles.Role;
                        });
                    });
                });
                //*** Admin ***
                options.AddPolicy("Admin", policy => {
                    policy.RequireAssertion(context => {
                        return context.User.HasClaim(c => {
                            return c.Type == Roles.Role && c.Value == Roles.Admin;
                        });
                    });
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
