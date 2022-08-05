using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Authorize;
using NetCoreSample.Models;
using NetCoreSample.Tools;
using Serilog;
using Serilog.Core;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("UserApi/[action]")]
    // [Authorize(Roles.Admin)]
    // [Authorize(AuthenticationSchemes = "CookieForWebApi")]
    public class UserController : BaseApiController
    {
        ILogger _logger;
        public UserController(BaseContext dbContext, ILogger logger) : base(dbContext)
        {
            _logger = logger;
        }

        [HttpPost]
        [IsNotLoginFilter]
        [AllowAnonymous]
        public IActionResult Login(User user)
        {
            // var DbUser = Users.Where(_ => _.Email == user.Email).FirstOrDefault();
            var DbUser = _dbContext.User.Where(_ => _.Name == user.Name).FirstOrDefault();
            if (DbUser == null)
                throw new Exception("查無使用者");

            if (user.Password.VerifyPassword(DbUser.Password))
            {
                var ClaimPriciple = new ClaimsPrincipal();
                var IdentityForView = new ClaimsIdentity("Cookie");
                IdentityForView.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                IdentityForView.AddClaim(new Claim("UserName", DbUser.Name, ClaimValueTypes.String));
                IdentityForView.AddClaim(new Claim("UserId", DbUser.UserId.ToString(), ClaimValueTypes.String));
                ClaimPriciple.AddIdentity(IdentityForView);
                HttpContext.User = ClaimPriciple;
                HttpContext.SignInAsync("CookieForView", HttpContext.User);

                var IdentityForWebApi = new ClaimsIdentity("Cookie");
                IdentityForWebApi.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                IdentityForWebApi.AddClaim(new Claim("UserName", DbUser.Name, ClaimValueTypes.String));
                IdentityForWebApi.AddClaim(new Claim("UserId", DbUser.UserId.ToString(), ClaimValueTypes.String));
                ClaimPriciple.AddIdentity(IdentityForWebApi);
                HttpContext.User = ClaimPriciple;
                HttpContext.SignInAsync("CookieForWebApi", HttpContext.User);

                return Ok();
            }
            throw new Exception("密碼錯誤");
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var Users = _dbContext.User.ToList();
            return Ok(Users);
        }

        [HttpPost]
        public IActionResult AddUser()
        {
            var User = new User()
            {
                // Email = $"{StringTool.GenerateString(8)}@gmail.com",
                Name = StringTool.GenerateString(5),
                Password = "p@ssWord"

            };
            _dbContext.User.Add(User);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("{userId:int}")]
        public IActionResult GetPostOfUser(int userId)
        {
            if (_dbContext.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            // var User = _dbContext.User.Include("Posts").Select(_ => new { _.UserId, _.Posts})
            //             .FirstOrDefault(_ => _.UserId == userId);
            var posts = _dbContext.Post.Where(_ => _.UserId == userId).ToList();

            return Ok(posts);
        }

        [HttpPost("{userId:int}")]
        [AllowAnonymous]
        public IActionResult GetTagOfUser(int userId)
        {
            if (_dbContext.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            // _logger.Debug(_dbContext.User.Where(_ => _.UserId == userId)
            //                 .SelectMany(tl => tl.Posts.SelectMany(p => p.PostTags.Select(pt => pt.Tag)))
            //                 .Select(_ => new { _.TagId, _.TagName }).Distinct().ToList().ToString());
            var Tags = _dbContext.User.Where(_ => _.UserId == userId)
                .SelectMany(tl => tl.Posts.SelectMany(p => p.PostTags.Select(pt => pt.Tag)))
                .Select(_ => new { _.TagId, _.TagName }).Distinct().ToList();
            return Ok(Tags);
        }
    }
}