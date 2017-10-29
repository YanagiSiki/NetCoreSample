using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using MongoDB.Driver;

namespace NetCoreSample.Controllers
{
    public class HomeController : BaseController
    {
        IMongoDatabase _db = _mongodbRepository._database;
        public ActionResult Index()
        {
            var users = _db.GetCollection<User>("User");

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
