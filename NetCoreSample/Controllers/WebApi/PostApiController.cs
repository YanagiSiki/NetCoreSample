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
        private BaseContext _dbContext;
        public PostApiController(BaseContext dbContext)
        {
            _dbContext = _dbContext?? dbContext;
        }

        [HttpGet]
        public IActionResult GetPost()
        {
            var posts = _dbContext.Post.ToList();
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult UpdatePost([FromBody] Post post)
        {
            var Post = _dbContext.Post.AsNoTracking().SingleOrDefault(_ => _.PostId == post.PostId);
            if (Post == null)return Ok("Post Not Found");

            var PostTag = _dbContext.PostTag.AsNoTracking().Where(_ => _.PostId == post.PostId).ToList();
            if (PostTag.IsNotNull())
                _dbContext.PostTag.RemoveRange(PostTag);
            if (post.PostTags.IsNotNull())
                _dbContext.PostTag.AddRange(post.PostTags.ToList());
            _dbContext.Post.Update(post);
            _dbContext.SaveChanges();

            return Ok();
        }

    }

}