using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("UserApi/[action]")]
    [AllowAnonymous]
    public class UserController : Controller
    {
        private BaseContext _dbContext;
        public UserController(BaseContext dbContext)
        {
            _dbContext = _dbContext ?? dbContext;
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
        public IActionResult GetPostOfUser(int userId = 0)
        {
            if (_dbContext.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            // var User = _dbContext.User.Include("Posts").Select(_ => new { _.UserId, _.Posts})
            //             .SingleOrDefault(_ => _.UserId == userId);
            var posts = _dbContext.Post.Where(_ => _.UserId == userId).ToList();

            return Ok(posts);
        }

        [HttpPost("{userId:int}")]
        public IActionResult GetTagOfUser(int userId = 0)
        {
            if (_dbContext.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            var Tags = _dbContext.User.Include("Posts.PostTags.Tag").Where(_ => _.UserId == userId)
                .SelectMany(tl => tl.Posts.SelectMany(p => p.PostTags.Select(pt => pt.Tag)))
                .Select(_ => new { _.TagId, _.TagName }).Distinct().ToList();
            return Ok(Tags);
        }
    }
}