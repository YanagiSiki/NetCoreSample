﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetCoreSample.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSample.Controllers
{
    //[AllowAnonymous]
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
        [Authorize("NotLogin")]
        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [Authorize("NotLogin")]
        public async Task<ActionResult> Login(User user)
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
            
            if (user.Password.ValidatePassword(dbUser.Password)) {
                User.AddIdentity(new ClaimsIdentity(new Claim[] {
                    new Claim(Roles.Role, Roles.Admin)
                }));

                await HttpContext.SignInAsync("UserLog", User);
                return View(user);
            }
            throw new Exception("密碼錯誤");
        }

        [HttpGet]
        [Authorize("NotLogin")]
        public ActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [Authorize("NotLogin")]
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

        [Authorize("Admin")]
        public ActionResult About() {
            return View();
        }
    }
}
