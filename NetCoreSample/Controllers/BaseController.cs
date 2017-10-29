using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSample.Controllers
{
    public class BaseController : Controller
    {
        protected static MongodbRepository _mongodbRepository = new MongodbRepository();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var x = context.ActionDescriptor.AttributeRouteInfo;
            if (!context.ModelState.IsValid)
            {
                throw new SampleException(context.ModelState);
            }

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