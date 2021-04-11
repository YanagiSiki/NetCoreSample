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
    public class BaseApiController : Controller
    {
        //protected static MongodbRepository _mongodbRepository = new MongodbRepository();        
        protected BaseContext _dbContext;

        public BaseApiController(BaseContext dbContext)
        {
            _dbContext = _dbContext ?? dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var controller = context.Controller as Controller;
                var errorMessage = ErrorHandle(context.ModelState);
                context.Result = controller.BadRequest(errorMessage);
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
                context.Result = controller.BadRequest(context.Exception.Message);
            }
        }
    }
}