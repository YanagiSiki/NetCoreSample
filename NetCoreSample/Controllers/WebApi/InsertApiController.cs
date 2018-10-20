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
        private NpgsqlContext _npgsql;
        public InsertApiController(NpgsqlContext npgsql)
        {
            _npgsql = _npgsql?? npgsql;
        }

        [HttpPost]
        public IActionResult InsertPost()
        {
            var Users = _npgsql.User.ToList();

            for (var i = 0; i < 5; i++)
            {
                var user = Users.GetRandomItem();
                var post = new Post()
                {
                    PostTitle = StringTool.GenerateString(5),
                    UserId = user.UserId
                };
                _npgsql.Post.Add(post);
            }
            _npgsql.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult InsertTag()
        {
            for (var i = 0; i < 3; i++)
            {
                var Tag = new Tag() { TagName = StringTool.GenerateString(3) };
                _npgsql.Tag.Add(Tag);
            }
            _npgsql.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult InsertPostTag()
        {
            var Posts = _npgsql.Post.ToList();
            var Tags = _npgsql.Tag.ToList();
            var PostTags = _npgsql.PostTag.ToList();

            for (var i = 0; i < 3; i++)
            {
                var post = Posts.GetRandomItem();
                var tag = Tags.GetRandomItem();
                var postTag = new PostTag() { PostId = post.PostId, TagId = tag.TagId };
                if (PostTags.Any(_ => _.PostId == postTag.PostId && _.TagId == postTag.TagId))
                    continue;
                _npgsql.PostTag.Add(postTag);
            }

            _npgsql.SaveChanges();
            return Ok();
        }
    }

}