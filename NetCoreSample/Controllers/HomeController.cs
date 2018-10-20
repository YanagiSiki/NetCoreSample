﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Helper;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers
{
    //API Key ID: 4AIWnYxRQuCalJLR-hV26A
    //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
    //[AllowAnonymous]
    public class HomeController : BaseController
    {
        public HomeController(NpgsqlContext npgsql): base(npgsql)
        {

        }

        public IActionResult Index()
        {
            var Users = _npgsql.User.ToAsyncEnumerable();
            return View();
        }

        [HttpGet]
        [IsNotLoginFilter]
        public IActionResult Login()
        {
            return Ok();
            return View(new User());
        }

        [HttpPost]
        [IsNotLoginFilter]
        public async Task<IActionResult> Login(User user)
        {
            var Users = _npgsql.User;
            var DbUser = Users.Where(_ => _.Email == user.Email).FirstOrDefault();

            if (DbUser == null)
                throw new Exception("查無使用者");

            if (user.Password.VerifyPassword(DbUser.Password))
            {
                var ClaimPriciple = new ClaimsPrincipal();
                var Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                Identity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                Identity.AddClaim(new Claim("UserName", DbUser.Name, ClaimValueTypes.String));
                Identity.AddClaim(new Claim("UserEmail", DbUser.Email, ClaimValueTypes.Email));
                ClaimPriciple.AddIdentity(Identity);
                HttpContext.User = ClaimPriciple;
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, HttpContext.User);
                //return Redirect("/Home");
                return RedirectToAction("index", "Home");
            }
            throw new Exception("密碼錯誤");
        }

        [HttpGet]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home");
        }

        [HttpGet]
        [IsNotLoginFilter]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [IsNotLoginFilter]
        public IActionResult Register(User User)
        {
            if (_npgsql.User.Where(u => u.Email == User.Email).Any())
            {
                WarningMessages.Add("Email已註冊過！");
                throw new Exception("Email已註冊過！");
            }
            User.Password = User.Password.HashPassword();
            User.Active = false;
            User.VerifyCode = StringTool.GenerateString(10);
            //SendGridHelper.SendEmailAsync();
            //SendGridHelper.SendVerifyCodeAsync(user.Email, user.Name, user.VerifyCode);

            _npgsql.User.Add(User);
            _npgsql.SaveChanges();
            SucessMessages.Add("成功註冊La！");
            return Redirect("/Home");
        }

        [HttpGet]
        [IsNotLoginFilter]
        public IActionResult VerifyAccount(string email, string userName, string code)
        {
            var Dbusers = _npgsql.User.Where(u => u.Email == email && u.Name == userName);
            if (Dbusers.Any())
            {
                var Dbuser = Dbusers.First();
                if (Dbuser.Active)
                {
                    WarningMessages.Add("已啟用");
                    return Redirect("/Home");
                }
                Dbuser.Active = true;
                _npgsql.User.Update(Dbuser);
                _npgsql.SaveChanges();
                SucessMessages.Add("成功啟用");
                return Redirect("/Home");

            }
            throw new Exception("Code有誤");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles.Admin)]
        public IActionResult About()
        {
            return View();
        }

        [Authorize(Roles.Admin)]
        public IActionResult Contact()
        {
            return View();
        }

        // public ActionResult TestInsert()
        // {
        //     //var user = _Npgsql.User.Where(u => u.Email == "QQ").First();
        //     var user = _Npgsql.User.Include(u => u.InterviewExperience).Where(u => u.Email == "QQ").First();
        //     var ie = new InterviewExperience()
        //     {
        //         Experience = StringTool.GenerateString(10),
        //         InterviewDate = DateTime.UtcNow.AddHours(08),
        //         UserId = user.UserId,
        //     };
        //     _Npgsql.InterviewExperience.Add(ie);
        //     _Npgsql.SaveChanges();
        //     return View("Index");
        // }

        public IActionResult TestSengrid()
        {
            //SendGridHelper.SendEmailAsync();
            return Ok();
        }
    }
}