using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetCoreSample.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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
        //[Authorize("NotLogin")]
        [IsNotLoginFilter]
        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        //[Authorize("NotLogin")]
        [IsNotLoginFilter]
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
                var ClaimPriciple = new ClaimsPrincipal();
                var Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                Identity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                //HttpContext.User.AddIdentity(Identity);
                ClaimPriciple.AddIdentity(Identity);
                //User.AddIdentity(Identity);
                HttpContext.User = ClaimPriciple;
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, HttpContext.User);
                return View(user);
            }
            throw new Exception("密碼錯誤");
        }

        [HttpGet]
        //[Authorize("NotLogin")]
        [IsNotLoginFilter]
        public ActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        //[Authorize("NotLogin")]
        [IsNotLoginFilter]
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
