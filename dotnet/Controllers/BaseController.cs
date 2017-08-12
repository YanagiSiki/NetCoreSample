using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using dotnet.Helper;
using System.Data.SqlClient;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace dotnet.Controllers
{
    public class BaseController : Controller
    {
        protected static MongodbRepository _mongodbRepository = new MongodbRepository();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var x = context.ActionDescriptor.AttributeRouteInfo;
            //處理欄位驗證
            //if (!context.ModelState.IsValid)
            //{
            //    // 例外處理
            //    context.Result = new ViewResult
            //    {
            //        ViewName = context,
            //        ViewData = context.ActionDescriptor,
            //        TempData = context.Controller.TempData
            //    };
            //}

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                Console.WriteLine(filterContext.Exception);
                filterContext.ExceptionHandled = true;

                ViewBag.ss = "12";
                var controller = filterContext.Controller as Controller;
                
                controller.ViewBag.Exception = "Foo message";

                filterContext.Result = controller.View();
                
                //filterContext.Result = new ViewResult
                //{
                //    ViewName = Url.Action((string)filterContext.RouteData.Values["Controller"], (string)filterContext.RouteData.Values["action"]),
                //    ViewData =
                //    //{ {"Exception", filterContext.Exception } },
                //};
            }

        }
    }
}