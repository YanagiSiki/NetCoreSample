using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("api/[action]")]
    [AllowAnonymous]
    public class UserController: Controller
    {
        private NpgsqlContext _npgsql;
        public UserController(NpgsqlContext npgsql)
        {
            if(_npgsql == null) _npgsql = npgsql;
        }

        [HttpGet]
        public IActionResult GetUser(string id1, string id2, string id3)
        {
            return Ok(new string[]{id1, id2, id3});
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _npgsql.User.Where(u => (user.Name.IsNullOrEmpty() || u.Name == user.Name));
            return Ok(user);
        }

        
    }
}