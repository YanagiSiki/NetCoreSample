using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Models;
using NetCoreSample.Tools;
using Newtonsoft.Json.Serialization;

namespace NetCoreSample
{
    /* 
     * 建置專案
     * dotnet build
     * 執行專案
     * dotnet run
     * 發布專案
     * dotnet publish -o ../Publish
     * dotnet publish -c Release -r <RID> --self-contained true  //linux-x64、ubuntu.14.04-x64、win10-x64 或 osx.10.12-x64
     * dotnet publish -c Release -r win10-x64 --self-contained true -o ../Publish
     * 更新套件
     * dotnet restore
     */
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
            services.AddMvc().AddJsonOptions(options =>
            {
                //防止無限迴圈
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //json字首固定大寫
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddEntityFrameworkNpgsql();
            services.AddEntityFrameworkMySql();
            services.AddScoped<BaseContext, HerokuNpgContext>();
            // services.AddScoped<BaseContext, MySQLContext>();
            // services.AddScoped<BaseContext, OracleMySQLContext>();

            services.AddCustomPolicyExtend();
            services.AddCustomAuthExtend();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            var IsHost = Environment.GetEnvironmentVariable("IsHost");
            if (IsHost?.ToLower() == "true")
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedProto
                });

                app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
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