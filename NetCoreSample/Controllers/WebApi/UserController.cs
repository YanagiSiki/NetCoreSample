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
    [Route("api/[action]")]
    // [AllowAnonymous]
    public class UserController : Controller
    {
        private NpgsqlContext _npgsql;
        public UserController(NpgsqlContext npgsql)
        {
            if (_npgsql == null)_npgsql = npgsql;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            // return Ok(new string[]{id1, id2, id3});
            return Ok(_npgsql.User.ToList());
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _npgsql.User.Where(u =>(user.Name.IsNullOrEmpty() || u.Name == user.Name));
            return Ok(user);
        }

        [HttpPut]
        public ActionResult Update()
        {

            return RedirectToAction("GetUser");
        }

        [HttpPost]
        public ActionResult GetInterviewEx()
        {
            /* https://docs.microsoft.com/zh-tw/ef/core/querying/related-data */
            return Ok(new
            {
                UserWithEx = _npgsql.User.Include("InterviewExperience").ToList(),
                    UserAll = _npgsql.User.ToList(),
                    ExAll = _npgsql.InterviewExperience.Include(_ => _.User).ToList(),
                    ExWithUser = _npgsql.InterviewExperience.Include("User").ToList()
            });
        }


        public ActionResult TestInsert()
        {
            //var user = _Npgsql.User.Where(u => u.Email == "QQ").First();
            var user = _npgsql.User.Include(u => u.InterviewExperience).Where(u => u.Email == "QQ").First();
            var ie = new InterviewExperience()
            {
                Experience = StringTool.GenerateString(10),
                InterviewDate = DateTime.UtcNow.AddHours(08),
                UserId = user.UserId,
            };
            _npgsql.InterviewExperience.Add(ie);
            _npgsql.SaveChanges();
            return Ok();
        }

        public ActionResult TestSengrid()
        {

            //SendGridHelper.SendEmailAsync();
            return Ok();
        }

        public ActionResult TestRelationShip()
        {
            // var post = _Npgsql.Post.Where(_ => _.PostId == 1).First();
            // var posttags = _Npgsql.PostTag.Where(_ => _.PostId == post.PostId).ToList();
            // var tags = _Npgsql.Tag.Where(_ => posttags.Any(i => i.TagId == _.TagId)).ToList();
            // var tmp = tags.First().TagName;
            // /*** https://docs.microsoft.com/zh-tw/ef/core/modeling/relationships ***/
            // _Npgsql.Post.Where(p => p.PostId == 1).Include("PostTags.Tag");
            return Ok();
        }
    }
}