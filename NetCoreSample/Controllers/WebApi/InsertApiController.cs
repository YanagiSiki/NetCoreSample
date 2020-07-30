using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Authorize;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("Api/[action]")]
    // [WebApiAuthorize]
    [Authorize(Roles.Admin)]
    [Authorize(AuthenticationSchemes = "CookieForWebApi")]
    public class InsertApiController : BaseApiController
    {
        public InsertApiController(BaseContext dbContext) : base(dbContext) { }

        [HttpPost]
        public IActionResult InsertPost()
        {
            var Users = _dbContext.User.Take(10).ToList();

            for (var i = 0; i < 5; i++)
            {
                var user = Users.GetRandomItem();
                var post = new Post()
                {
                    PostTitle = StringTool.GenerateString(5),
                    UserId = user.UserId,
                    PostDate = DateTime.Now
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
            var Posts = _dbContext.Post.Take(5).ToList();
            var Tags = _dbContext.Tag.Take(5).ToList();
            var PostTags = _dbContext.PostTag.Take(5).ToList();

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

        [HttpPost]
        public IActionResult TestGitCount([FromBody] GitPostParameter GitPostParameter)
        {

            return Ok();
        }
    }

    public class GitPostParameter
    {
        public string rev { get; set; }
        public string branch { get; set; }
        public string repo { get; set; }

    }

}