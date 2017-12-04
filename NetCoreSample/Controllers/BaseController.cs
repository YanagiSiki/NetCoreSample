using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetCoreSample.Controllers
{
    public class BaseController : Controller
    {
        protected static MongodbRepository _mongodbRepository = new MongodbRepository();
        protected static MSSQLDbContext _MSSQLDbContext = new MSSQLDbContext();

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
                context.Result = controller.View(TempData["CurrentModel"]);
            }
            TempData["CurrentModel"] = null;
        }
    }
}