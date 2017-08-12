using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using Dapper;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnet.Controllers
{
    public class HomeController : BaseController
    {
        IMongoDatabase _db = _mongodbRepository._database;
        public ActionResult Index()
        {
            var users = _db.GetCollection<User>("User");
           
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
