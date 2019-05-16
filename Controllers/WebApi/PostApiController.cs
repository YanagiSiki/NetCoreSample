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
    [Route("PostApi/[action]")]
    // [Authorize(Roles.Admin)]
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
        [Authorize(Roles.Admin)]
        public IActionResult UpdatePost(Post post)
        {
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                if (_dbContext.Post.All(_ => _.PostId != post.PostId))throw new Exception("Post Not Found");

                string UserId = HttpContext.User.Claims.SingleOrDefault(_ => _.Type == "UserId")?.Value;
                if (_dbContext.Post.FirstOrDefault(_ => _.PostId == post.PostId).UserId.ToString() != UserId)
                    throw new Exception("You are not owner !!");

                var Tags = post.PostTags?.Select(_ => _.Tag).ToList();

                /*** post ***/
                post.PostTags = null;
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
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles.Admin)]
        public IActionResult InsertPost(Post post)
        {
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                var Tags = post.PostTags?.Select(_ => _.Tag).ToList();

                /*** post ***/
                var UserId = HttpContext.User.Claims.First(_ => _.Type == "UserId").Value;
                post.UserId = int.Parse(UserId);
                post.PostTags = null;
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
            return Ok();
        }

        [HttpGet]
        public IActionResult GetTagsOfPost(int postId)
        {
            var PostTags = _dbContext.PostTag.Include(_ => _.Tag).Where(_ => _.PostId == postId).ToList();
            var Tags = PostTags.Select(_ => _.Tag).ToList();
            return Ok(Tags);
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            var AllTags = _dbContext.Tag.ToList();
            return Ok(AllTags);
        }
    }

}