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
            //var users = _db.GetCollection<User>("User");
            
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var users = _db.GetCollection<User>("User");
            try {
                //users.InsertOne(user);
            }
            catch(Exception ex) {

            }
            
            return View(user);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
