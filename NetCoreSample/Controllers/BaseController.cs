using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace NetCoreSample.Controllers
{
    public class BaseController : Controller
    {
        //protected static MongodbRepository _mongodbRepository = new MongodbRepository();
        protected static NpgsqlContext _MSSQLDbContext = new NpgsqlContext();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            TempData["CurrentModel"] = context.ActionArguments.Values.Count == 1 ? context.ActionArguments.Values.FirstOrDefault() : null;
            if (!context.ModelState.IsValid)
            {
                var controller = context.Controller as Controller;
                var errorMessage =  ErrorHandle(context.ModelState);
                TempData["ErrorMessage"] = errorMessage;
                context.Result = controller.View(TempData["CurrentModel"]);
            }

        }

        protected List<string> ErrorHandle(ModelStateDictionary modelState)
        {
            return modelState.Keys
            .SelectMany(key => modelState[key].Errors.Select(x => x.ErrorMessage)).ToList();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                Console.WriteLine(context.Exception);
                context.ExceptionHandled = true;
                var controller = context.Controller as Controller;
                TempData["ErrorMessage"] = new List<string>() { context.Exception.Message };

                // https://gist.github.com/ygrenier/4e46b5de3e4a77ea62fe5f0d6488fa2a
                /*** 若此Action沒有對應的View，就回到首頁，並顯示 Exception Message ***/
                var services = controller.HttpContext.RequestServices;
                var viewEngine = services.GetRequiredService<ICompositeViewEngine>();
                var thisView = viewEngine.GetView(null, controller.RouteData.Values["action"].ToString(), true);
                if (!thisView.Success)
                    context.Result = controller.View("~/index",TempData["CurrentModel"]);
                else
                context.Result = controller.View(TempData["CurrentModel"]);
            }
            TempData["CurrentModel"] = null;
        }
    }
}