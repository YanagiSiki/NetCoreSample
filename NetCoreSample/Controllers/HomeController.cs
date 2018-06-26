using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Helper;
using NetCoreSample.Models;

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

        public ActionResult Index()
        {
            var users = _Npgsql.User.ToAsyncEnumerable();
            return View();
        }

        [HttpGet]
        [IsNotLoginFilter]
        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [IsNotLoginFilter]
        public async Task<ActionResult> Login(User user)
        {
            var users = _Npgsql.User;
            var dbUser = users.Where(_ => _.Email == user.Email).FirstOrDefault();

            if (dbUser == null)
                throw new Exception("查無使用者");

            if (user.Password.ValidatePassword(dbUser.Password))
            {
                var ClaimPriciple = new ClaimsPrincipal();
                var Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                Identity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                Identity.AddClaim(new Claim("UserName", dbUser.Name, ClaimValueTypes.String));
                Identity.AddClaim(new Claim("UserEmail", dbUser.Email, ClaimValueTypes.Email));
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
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home");
        }

        [HttpGet]
        [IsNotLoginFilter]
        public ActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [IsNotLoginFilter]
        public ActionResult Register(User user)
        {
            if (_Npgsql.User.Where(u => u.Email == user.Email).Any())
            {
                WarningMessages.Add("Email已註冊過！");
                throw new Exception("Email已註冊過！");
            }
            user.Password = user.Password.HashPassword();
            user.Active = false;
            user.VerifyCode = StringTool.GenerateString(10);
            //SendGridHelper.SendEmailAsync();
            SendGridHelper.SendVerifyCodeAsync(user.Email, user.Name, user.VerifyCode);

            _Npgsql.User.Add(user);
            _Npgsql.SaveChanges();
            SucessMessages.Add("成功註冊La！");
            return Redirect("/Home");
        }

        [HttpGet]
        [IsNotLoginFilter]
        public ActionResult VerifyAccount(string email, string userName, string code)
        {
            var dbusers = _Npgsql.User.Where(u => u.Email == email && u.Name == userName);
            if (dbusers.Any())
            {
                var dbuser = dbusers.First();
                if (dbuser.Active)
                {
                    WarningMessages.Add("已啟用");
                    return Redirect("/Home");
                }
                dbuser.Active = true;
                _Npgsql.User.Update(dbuser);
                _Npgsql.SaveChanges();
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
        public ActionResult About()
        {
            return View();
        }

        [Authorize(Roles.Admin)]
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult TestInsert()
        {
            var user = new User()
            {
                Name = StringTool.GenerateString(3),
                Email = StringTool.GenerateString(3),
                Password = StringTool.GenerateString(3).HashPassword()
            };

            if (_Npgsql.User.Where(u => u.Email == user.Email).Any())
                throw new Exception("Email已註冊過！");

            _Npgsql.User.Add(user);
            _Npgsql.SaveChanges();
            //user = _Npgsql.User.Where(u => u.Email == user.Email).FirstOrDefault();
            //var ie = new InterviewExperience()
            //{
            //    Experience = StringTool.GenerateString(10),
            //    InterviewDate = DateTime.UtcNow.AddHours(08),
            //    UserId = user.UserId,
            //};
            //_Npgsql.InterviewExperience.Add(ie);
            //_Npgsql.SaveChanges();
            return View("Index");
        }

        public ActionResult TestSengrid()
        {
            SendGridHelper.SendEmailAsync();
            return Ok();
        }
    }
}