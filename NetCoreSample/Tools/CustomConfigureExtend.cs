using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCoreSample.Helper;
using NetCoreSample.Models;

namespace NetCoreSample.Tools
{
    public static class CustomConfigureExtend
    {
        public static IServiceCollection AddCustomCookieAuthExtend(this IServiceCollection services)
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

        public static IServiceCollection AddCustomJwtAuthExtend(this IServiceCollection services)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper("JWToken");
            services.AddSingleton<JwtHelpers>();
            services.AddAuthentication("JWToken")
                .AddJwtBearer("JWToken", options =>
                {
                    // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                    options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                        // NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        // // // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                        // RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                        // 一般我們都會驗證 Issuer
                        ValidateIssuer = true,
                        ValidIssuer = configurationHelper.GetValue<string>("JwtSettings:Issuer"),

                        // 通常不太需要驗證 Audience
                        ValidateAudience = false,
                        //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

                        // 一般我們都會驗證 Token 的有效期間
                        ValidateLifetime = true,

                        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                        ValidateIssuerSigningKey = false,

                        // "1234567890123456" 應該從 IConfiguration 取得
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationHelper.GetValue<string>("JwtSettings:SignKey")))
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