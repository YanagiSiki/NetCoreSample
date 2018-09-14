using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Models;
using NetCoreSample.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            /*
             * https://stackoverflow.com/questions/34892509/controller-json-set-serialization-referenceloophandling/36633265
             * 防止RelationShip include之後無限循環參考
             */
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<NpgsqlContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = new PathString("/Home/Error");
                    options.LoginPath = new PathString("/Home/Login");
                    options.LogoutPath = new PathString("/Home/Logout");
                });

            // services.AddAuthorization(options =>
            // {
            //     //*** Admin ***
            //     options.AddPolicy("Admin", policy =>
            //     {
            //         policy.RequireAssertion(context =>
            //         {
            //             return context.User.HasClaim(c =>
            //             {
            //                 return c.Type == Roles.Role && c.Value == Roles.Admin;
            //             });
            //         });
            //     });
            // });
            services.AddCustomConfigureExtend();
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