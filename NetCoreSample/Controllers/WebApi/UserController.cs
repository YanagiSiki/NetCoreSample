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
        private NpgsqlContext _npgsql;
        public UserController(NpgsqlContext npgsql)
        {
            _npgsql = _npgsql ?? npgsql;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var Users = _npgsql.User.ToList();
            return Ok(Users);
        }

        [HttpPost]
        public IActionResult AddUser()
        {
            var User = new User()
            {
                Email = $"{StringTool.GenerateString(8)}@gmail.com",
                Name = StringTool.GenerateString(5),
                Password = "p@ssWord"

            };
            _npgsql.User.Add(User);
            _npgsql.SaveChanges();
            return Ok();
        }

        [HttpPost("{userId:int}")]
        public IActionResult GetPostOfUser(int userId = 0)
        {
            if (_npgsql.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            var User = _npgsql.User.Include("Posts.PostTags.Tag").SingleOrDefault(_ => _.UserId == userId);
            return Ok(User);
        }

        [HttpPost("{userId:int}")]
        public IActionResult GetTagOfUser(int userId = 0)
        {
            if (_npgsql.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            var Tags = _npgsql.User.Include("Posts.PostTags.Tag").Where(_ => _.UserId == userId)
                .SelectMany(tl => tl.Posts.SelectMany(p => p.PostTags.Select(pt => pt.Tag)))
                .Distinct().ToList();

            return Ok(Tags);
        }
    }
}