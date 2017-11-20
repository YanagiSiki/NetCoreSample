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
            var dbUser = users.Find(i => i.Email == user.Email).FirstOrDefault();
            if (dbUser == null) {
                throw new Exception("查無使用者");
            }
            if(user.Password.ValidatePassword(dbUser.Password))
                return View(user);
            throw new Exception("密碼錯誤");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            var users = _db.GetCollection<User>("User");
            user.Password = user.Password.HashPassword();
            users.InsertOneAsync(user, new InsertOneOptions() {

            });
            return View(user);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
