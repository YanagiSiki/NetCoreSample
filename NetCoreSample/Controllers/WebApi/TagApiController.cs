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
    [Route("TagApi/[action]")]
    [Authorize(Roles.Admin)]
    public class TagApiController : BaseApiController
    {
        public TagApiController(BaseContext dbContext, JwtHelpers jwtHelpers) : base(dbContext, jwtHelpers) { }

        [AllowAnonymous]
        public IActionResult GetTags()
        {
            var tmp = _dbContext.Tag.Select(_ => new { Tag = new { _.TagId, _.TagName }, Count = _.PostTags.Count() }).ToList();
            return Ok(tmp);
        }
        // public IActionResult GetPostOfTag(int tagId)
        // {
        //     if (tagId == 0 || _dbContext.Tag.All(_ => _.TagId != tagId))throw new Exception("Tag Not Found");

        //     var Posts = _dbContext.Tag.Include("PostTags.Post").Where(_ => _.TagId == tagId)
        //         .SelectMany(pts => pts.PostTags.Select(pt => pt.Post)).ToList();
        //     return Ok(Posts);
        // }

    }

}