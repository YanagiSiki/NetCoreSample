using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("PostApi/[action]")]
    [AllowAnonymous]
    public class PostApiController : Controller
    {
        private NpgsqlContext _npgsql;
        public PostApiController(NpgsqlContext npgsql)
        {
            _npgsql = _npgsql?? npgsql;
        }

        [HttpGet]
        public IActionResult GetPost()
        {
            var posts = _npgsql.Post.ToList();
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult UpdatePost([FromBody] Post post)
        {
            var Post = _npgsql.Post.AsNoTracking().SingleOrDefault(_ => _.PostId == post.PostId);
            if (Post == null)return Ok("Post Not Found");

            var PostTag = _npgsql.PostTag.AsNoTracking().Where(_ => _.PostId == post.PostId).ToList();
            if (PostTag.IsNotNull())
                _npgsql.PostTag.RemoveRange(PostTag);
            if (post.PostTags.IsNotNull())
                _npgsql.PostTag.AddRange(post.PostTags.ToList());
            _npgsql.Post.Update(post);
            _npgsql.SaveChanges();

            return Ok();
        }

    }

}