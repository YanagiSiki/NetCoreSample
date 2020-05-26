using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Models;

namespace NetCoreSample.Controllers
{
    public class BaseController : Controller
    {
        //protected static MongodbRepository _mongodbRepository = new MongodbRepository();        
        protected BaseContext _dbContext;

        /*** Sucess、Warning可自定；Error會直接抓Exception ***/
        protected List<string> SucessMessages = new List<string>();
        protected List<string> WarningMessages = new List<string>();

        public BaseController(BaseContext dbContext)
        {
            _dbContext = _dbContext ?? dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SucessMessages = new List<string>();
            WarningMessages = new List<string>();
            if (!context.ModelState.IsValid)
            {
                var controller = context.Controller as Controller;
                var errorMessage = ErrorHandle(context.ModelState);
                TempData["ErrorMessage"] = errorMessage.ToArray();
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
                TempData["ErrorMessage"] = new string[] { context.Exception.Message };
                context.Result = controller.Redirect("/Home/Error");
            }
            if (SucessMessages.Any())TempData["SucessMessage"] = SucessMessages.ToArray();
            if (WarningMessages.Any())TempData["WarningMessage"] = WarningMessages.ToArray();
        }
    }
}