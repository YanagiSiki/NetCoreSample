using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreSample.Controllers
{
    [AllowAnonymousAttribute]
    public class HomeController : BaseController
    {
        IMongoDatabase _mongoDb = _mongodbRepository._database;
        //MSSQLDbContext _MSSQLDb = _MSSQLDbContext;

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
            //*** for mongodb ***
            var users = _mongoDb.GetCollection<User>("User");
            var dbUser = users.Find(i => i.Email == user.Email).FirstOrDefault();

            //*** for MSSQL ***
            //var users = _MSSQLDb.User;
            //var dbUser = users.Where(i => i.Email == user.Email).FirstOrDefault();

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
            //*** for mongodb ***
            var users = _mongoDb.GetCollection<User>("User");
            user.Password = user.Password.HashPassword();
            users.InsertOneAsync(user);

            //*** for MSSQL ***
            //_MSSQLDb.Set<User>().Add(user);
            //_MSSQLDb.User.Add(user);
            //_MSSQLDb.SaveChanges();

            return View(user);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
