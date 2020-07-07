using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Models;
using NetCoreSample.Tools;
namespace NetCoreSample.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles.Admin)]
    public class BatchController : BaseController
    {
        public BatchController(BaseContext dbContext) : base(dbContext) { }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok("Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            // if (str.IsNullOrEmpty())return BadRequest("str is null");
            // BackgroundJob.Enqueue(() => Console.WriteLine($"Hello {str}"));
            var task = new Task(() =>
            {
                for (var i = 1; i < 10000; i++)
                {
                    var str = StringTool.GenerateString(5);
                    BackgroundJob.Enqueue(() => Console.WriteLine($"Hello {str}"));
                    // RecurringJob.AddOrUpdate("流程A", () => Console.Write($"Hello {str}"), Cron.Minutely);
                }
            });
            await task;
            return Ok("Test");
        }
    }
}