using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Authorize;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("UserApi/[action]")]
    [Authorize(Roles.Admin)]
    public class UserController : BaseApiController
    {
        public UserController(BaseContext dbContext, JwtHelpers jwtHelpers) : base(dbContext, jwtHelpers) { }

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

                var userClaimsIdentity = new ClaimsIdentity("JWToken");
                userClaimsIdentity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                userClaimsIdentity.AddClaim(new Claim("UserName", DbUser.Name, ClaimValueTypes.String));
                userClaimsIdentity.AddClaim(new Claim("UserId", DbUser.UserId.ToString(), ClaimValueTypes.String));

                var token = _jwtHelpers.GenerateToken(userClaimsIdentity);
                return Ok(token);
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
        public IActionResult GetTagOfUser(int userId)
        {
            if (_dbContext.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            var Tags = _dbContext.User.Where(_ => _.UserId == userId)
                .SelectMany(tl => tl.Posts.SelectMany(p => p.PostTags.Select(pt => pt.Tag)))
                .Select(_ => new { _.TagId, _.TagName }).Distinct().ToList();
            return Ok(Tags);
        }
    }
}