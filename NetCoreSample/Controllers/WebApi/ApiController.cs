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
    public class ApiController : Controller
    {
        private NpgsqlContext _npgsql;
        public ApiController(NpgsqlContext npgsql)
        {
            if (_npgsql == null)_npgsql = npgsql;
        }

        public IActionResult InsertPost()
        {
            var users = _npgsql.User.ToList();

            for (var i = 0; i < 5; i++)
            {
                var user = users.GetRandomItem();
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

    }

}