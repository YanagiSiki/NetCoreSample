using System;
using System.Collections.Generic;
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
    [Route("[controller]/[action]")]
    public class HomeController : BaseController
    {
        public HomeController(BaseContext dbContext) : base(dbContext)
        {

        }

        [Route("~/")]
        // [Route("/Home/{page?}")]
        [Route("/Home/Index/{page?}")]
        public IActionResult Index(int page)
        {
            page = page > 0 ? page : 1;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = 20;
            ViewBag.PageRange = 2;
            var Posts = _dbContext.Post.Pagination(page);
            return View(Posts);
        }

        [HttpGet]
        [IsNotLoginFilter]
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [IsNotLoginFilter]
        public async Task<IActionResult> Login(User user)
        {
            var Users = _dbContext.User;
            // var DbUser = Users.Where(_ => _.Email == user.Email).FirstOrDefault();
            var DbUser = Users.Where(_ => _.Name == user.Name).FirstOrDefault();
            if (DbUser == null)
                throw new Exception("查無使用者");

            if (user.Password.VerifyPassword(DbUser.Password))
            {
                var ClaimPriciple = new ClaimsPrincipal();
                var Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                Identity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                Identity.AddClaim(new Claim("UserName", DbUser.Name, ClaimValueTypes.String));
                Identity.AddClaim(new Claim("UserId", DbUser.UserId.ToString(), ClaimValueTypes.String));
                ClaimPriciple.AddIdentity(Identity);
                HttpContext.User = ClaimPriciple;
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, HttpContext.User);
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

        [HttpGet("{postId?}")]
        public IActionResult Edit(int postId)
        {
            if (postId == 0)
            {
                return View(new Post()
                {
                    // PostId = 1,
                    PostTitle = "PostTitle",
                        PostContent = "# This is H1! \r\n PostContent",
                        //PostTags = PostTags
                });
            }

            var Post = _dbContext.Post.FirstOrDefault(_ => _.PostId == postId);
            if (Post == null)
                throw new Exception("Post Not Found");

            return View(Post);
        }

        //聽說只要增加webhook就可以在每次push完後，自動把code拉到伺服器，執行sh去deploey...?
        public IActionResult GitAutoPull()
        {
            //https://loune.net/2017/06/running-shell-bash-commands-in-net-core/
            var cmd = "";
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                }
            };
            process.Start();
            return Ok();
        }

        // [HttpGet]
        // [IsNotLoginFilter]
        // public IActionResult Register()
        // {
        //     return View(new User());
        // }
        // 
        // [HttpPost]
        // [IsNotLoginFilter]
        // public IActionResult Register(User User)
        // {
        //     // if (_dbContext.User.Where(u => u.Email == User.Email).Any())
        //     if (_dbContext.User.Where(u => u.Name == User.Name).Any())
        //     {
        //         WarningMessages.Add("Email已註冊過！");
        //         throw new Exception("Email已註冊過！");
        //     }
        //     User.Password = User.Password.HashPassword();
        //     // User.Active = false;
        //     // User.VerifyCode = StringTool.GenerateString(10);
        //     //SendGridHelper.SendEmailAsync();
        //     //SendGridHelper.SendVerifyCodeAsync(user.Email, user.Name, user.VerifyCode);

        //     _dbContext.User.Add(User);
        //     _dbContext.SaveChanges();
        //     SucessMessages.Add("成功註冊La！");
        //     return Redirect("/Home");
        // }

        // [HttpGet]
        // [IsNotLoginFilter]
        // public IActionResult VerifyAccount(string email, string userName, string code)
        // {
        //     var Dbusers = _dbContext.User.Where(u => u.Email == email && u.Name == userName);
        //     if (Dbusers.Any())
        //     {
        //         var Dbuser = Dbusers.First();
        //         if (Dbuser.Active)
        //         {
        //             WarningMessages.Add("已啟用");
        //             return Redirect("/Home");
        //         }
        //         Dbuser.Active = true;
        //         _dbContext.User.Update(Dbuser);
        //         _dbContext.SaveChanges();
        //         SucessMessages.Add("成功啟用");
        //         return Redirect("/Home");

        //     }
        //     throw new Exception("Code有誤");
        // }

    }
}