using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Authorize;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("PostApi/[action]")]
    [Authorize(Roles.Admin)]
    [Authorize(AuthenticationSchemes = "CookieForWebApi")]
    public class PostApiController : BaseApiController
    {
        public PostApiController(BaseContext dbContext) : base(dbContext) { }

        [HttpGet]
        // [AllowAnonymous]
        public IActionResult GetPost()
        {
            var posts = _dbContext.Post.ToList();
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult UpdatePost(Post post)
        {
            if (_dbContext.Post.AsNoTracking().All(_ => _.PostId != post.PostId))throw new Exception("Post Not Found");

            string UserId = HttpContext.User.Claims.FirstOrDefault(_ => _.Type == "UserId")?.Value;
            if (_dbContext.Post.AsNoTracking().FirstOrDefault(_ => _.PostId == post.PostId).UserId.ToString() != UserId)
                throw new Exception("You are not owner !!");

            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                var Tags = post.PostTags?.Select(_ => _.Tag).ToList();

                /*** post ***/
                post.PostTags = null;
                post.PostDate = DateTime.Now;
                _dbContext.Post.Update(post);
                _dbContext.SaveChanges();

                /*** tag ***/
                if (Tags.IsNotNull() && Tags.Any(_ => _.TagId == 0))
                {
                    _dbContext.Tag.AddRange(Tags.Where(_ => _.TagId == 0));
                    _dbContext.SaveChanges();
                }

                /*** posttags ***/
                var DbPostTags = _dbContext.PostTag.Where(_ => _.PostId == post.PostId);
                if (DbPostTags.IsNotNull())
                {
                    _dbContext.PostTag.RemoveRange(DbPostTags);
                    _dbContext.SaveChanges();
                }
                var PostTags = Tags?.Select(_ => { return new PostTag() { PostId = post.PostId, TagId = _.TagId }; }).ToList();
                if (PostTags.IsNotNull())
                {
                    _dbContext.PostTag.AddRange(PostTags);
                    _dbContext.SaveChanges();
                }

                transaction.Commit();
            }
            return Ok(post.PostId);
        }

        [HttpPost]
        public IActionResult InsertPost(Post post)
        {
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                var Tags = post.PostTags?.Select(_ => _.Tag).ToList();

                /*** post ***/
                var UserId = HttpContext.User.Claims.First(_ => _.Type == "UserId").Value;
                post.UserId = int.Parse(UserId);
                post.PostTags = null;
                post.PostDate = DateTime.Now;
                _dbContext.Post.Add(post);
                _dbContext.SaveChanges();

                /*** tag ***/
                if (Tags.IsNotNull() && Tags.Any(_ => _.TagId == 0))
                {
                    _dbContext.Tag.AddRange(Tags.Where(_ => _.TagId == 0));
                    _dbContext.SaveChanges();
                }

                /*** posttags ***/
                var PostTags = Tags?.Select(_ => { return new PostTag() { PostId = post.PostId, TagId = _.TagId }; }).ToList();
                if (PostTags.IsNotNull())
                {
                    _dbContext.PostTag.AddRange(PostTags);
                    _dbContext.SaveChanges();
                }
                transaction.Commit();
            }
            return Ok(post.PostId);
        }

        [HttpPost]
        public IActionResult DeletePost(int postId)
        {
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                if (_dbContext.Post.AsNoTracking().All(_ => _.PostId != postId))throw new Exception("Post Not Found");

                string UserId = HttpContext.User.Claims.FirstOrDefault(_ => _.Type == "UserId")?.Value;
                if (_dbContext.Post.AsNoTracking().FirstOrDefault(_ => _.PostId == postId).UserId.ToString() != UserId)
                    throw new Exception("You are not owner !!");

                _dbContext.Post.RemoveRange(_dbContext.Post.Where(_ => _.PostId == postId));
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            return Ok("刪除成功");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetTagsOfPost(int postId)
        {
            // 假設select、where條件沒有使用到該property，這時就要用include，ef core產生時才會帶入join table。
            // 反之，若select、where條件已經有使用了，則不需要使用include
            // 如果使用了，console log裡面會出現warning提醒，所以也不用太擔心。
            var PostTags = _dbContext.PostTag.Include(_ => _.Tag).Where(_ => _.PostId == postId).ToList();
            var Tags = PostTags.Select(_ => _.Tag).ToList();
            return Ok(Tags);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllTags()
        {
            var AllTags = _dbContext.Tag.ToList();
            return Ok(AllTags);
        }
    }

}