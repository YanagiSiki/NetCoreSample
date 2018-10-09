using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("UserApi/[action]")]
    [AllowAnonymous]
    public class UserController: Controller
    {
        private NpgsqlContext _npgsql;
        public UserController(NpgsqlContext npgsql)
        {
            if(_npgsql == null) _npgsql = npgsql;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var users = _npgsql.User.ToList();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _npgsql.User.Where(u => (user.Name.IsNullOrEmpty() || u.Name == user.Name));
            return Ok(user);
        }

        
    }
}