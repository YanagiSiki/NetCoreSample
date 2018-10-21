using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("Api/[action]")]
    [AllowAnonymous]
    public class InsertApiController : Controller
    {
        private BaseContext _dbContext;
        public InsertApiController(BaseContext dbContext)
        {
            _dbContext = _dbContext?? dbContext;
        }

        [HttpPost]
        public IActionResult InsertPost()
        {
            var Users = _dbContext.User.ToList();

            for (var i = 0; i < 5; i++)
            {
                var user = Users.GetRandomItem();
                var post = new Post()
                {
                    PostTitle = StringTool.GenerateString(5),
                    UserId = user.UserId
                };
                _dbContext.Post.Add(post);
            }
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult InsertTag()
        {
            for (var i = 0; i < 3; i++)
            {
                var Tag = new Tag() { TagName = StringTool.GenerateString(3) };
                _dbContext.Tag.Add(Tag);
            }
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult InsertPostTag()
        {
            var Posts = _dbContext.Post.ToList();
            var Tags = _dbContext.Tag.ToList();
            var PostTags = _dbContext.PostTag.ToList();

            for (var i = 0; i < 3; i++)
            {
                var post = Posts.GetRandomItem();
                var tag = Tags.GetRandomItem();
                var postTag = new PostTag() { PostId = post.PostId, TagId = tag.TagId };
                if (PostTags.Any(_ => _.PostId == postTag.PostId && _.TagId == postTag.TagId))
                    continue;
                _dbContext.PostTag.Add(postTag);
            }

            _dbContext.SaveChanges();
            return Ok();
        }
    }

}