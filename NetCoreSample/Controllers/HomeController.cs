using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //IMongoDatabase _mongoDb = _mongodbRepository._database;
        NpgsqlContext _Npgsql = _MSSQLDbContext;

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
            //*** for mongodb ***
            //var users = _mongoDb.GetCollection<User>("User");
            //var dbUser = users.Find(i => i.Email == user.Email).FirstOrDefault();

            //*** for MSSQL ***
            var users = _Npgsql.User;
            var dbUser = users.Where(i => i.Email == user.Email).FirstOrDefault();

            if (dbUser == null) {
                throw new Exception("查無使用者");
            }
            
            if (user.Password.ValidatePassword(dbUser.Password)) {
                var ClaimPriciple = new ClaimsPrincipal();
                var Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                Identity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                //HttpContext.User.AddIdentity(Identity);
                ClaimPriciple.AddIdentity(Identity);
                //User.AddIdentity(Identity);
                HttpContext.User = ClaimPriciple;
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, HttpContext.User);
                //return View(user);
                return Redirect("/Home");
            }
            throw new Exception("密碼錯誤");
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
            //*** for mongodb ***
            //var users = _mongoDb.GetCollection<User>("User");
            //user.Password = user.Password.HashPassword();
            //users.InsertOneAsync(user);

            //*** for MSSQL ***
            //_Npgsql.Set<User>().Add(user);
            _Npgsql.User.Add(user);
            _Npgsql.SaveChanges();

            return View(user);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles.Admin)]
        public ActionResult About() {
            return View();
        }

        public ActionResult TestInser() {
            var user = new User() {
                Name = StringTool.GenerateString(3),
                Email = StringTool.GenerateString(3),
                Password = StringTool.GenerateString(3)
            };

            if (_Npgsql.User.Where(u => u.Email == user.Email).Any())
                throw new Exception("Email已註冊過！");

            _Npgsql.User.Add(user);
            _Npgsql.SaveChanges();
            user = _Npgsql.User.Where(u => u.Email == user.Email).FirstOrDefault();
            var ie = new InterviewExperience()
            {
                Experience = StringTool.GenerateString(10),
                InterviewDate = DateTime.UtcNow.AddHours(08),
                UserId = user.UserId,
            };
            _Npgsql.InterviewExperience.Add(ie);
            _Npgsql.SaveChanges();
            return View("index");
        }
    }
}
