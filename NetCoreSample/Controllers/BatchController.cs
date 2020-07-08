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
using NetCoreSample.BatchJobs;
using NetCoreSample.Models;
using NetCoreSample.Tools;
namespace NetCoreSample.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles.Admin)]
    [Authorize(AuthenticationSchemes = "CookieForView")]
    public class BatchController : BaseController
    {
        public BatchController(BaseContext dbContext) : base(dbContext) { }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok("Index");
        }

        [AllowAnonymous]
        public IActionResult Test()
        {
            BatchJob.RunSomeJobs().Start();
            return Ok("Test");
        }
    }
}