using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Authorize;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers.WebApi
{
    [Route("Token/[controller]/[action]")]
    [Authorize(Roles.Admin)]
    [Authorize(AuthenticationSchemes = "JWToken")]
    public class UserApiController : BaseTokenApiController
    {
        public UserApiController(BaseContext dbContext, JwtHelpers jwtHelpers) : base(dbContext, jwtHelpers)
        {

        }

        /* 未取得token時，會回傳 401 UnAuth */
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetToken(User user)
        {
            var DbUser = _dbContext.User.Where(_ => _.Name == user.Name).FirstOrDefault();
            if (DbUser == null)
                throw new Exception("查無使用者");
            if (user.Password.VerifyPassword(DbUser.Password))
            {
                var userClaimsIdentity = new ClaimsIdentity("Token");
                userClaimsIdentity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                userClaimsIdentity.AddClaim(new Claim("UserName", DbUser.Name, ClaimValueTypes.String));
                userClaimsIdentity.AddClaim(new Claim("UserId", DbUser.UserId.ToString(), ClaimValueTypes.String));

                var token = _jwtHelpers.GenerateToken(userClaimsIdentity);
                return Ok(token);
            }
            throw new Exception("密碼錯誤");
        }

        /* 取得token，但該token並未有擁有相對應權限，回傳 403 Forbidden */
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetTokenButNoRole(User user)
        {
            var DbUser = _dbContext.User.Where(_ => _.Name == user.Name).FirstOrDefault();
            if (DbUser == null)
                throw new Exception("查無使用者");
            if (user.Password.VerifyPassword(DbUser.Password))
            {
                var userClaimsIdentity = new ClaimsIdentity("Token");
                // userClaimsIdentity.AddClaim(new Claim(Roles.Role, Roles.Admin, ClaimValueTypes.String));
                userClaimsIdentity.AddClaim(new Claim("UserName", DbUser.Name, ClaimValueTypes.String));
                userClaimsIdentity.AddClaim(new Claim("UserId", DbUser.UserId.ToString(), ClaimValueTypes.String));

                var token = _jwtHelpers.GenerateToken(userClaimsIdentity);
                return Ok(token);
            }
            throw new Exception("密碼錯誤");
        }

        /* Token/UserApi/GetTagOfUser/7 */
        [HttpPost("{userId:int}")]
        public IActionResult GetTagOfUser(int userId)
        {

            if (_dbContext.User.All(_ => _.UserId != userId))
                return Ok("User Not Found");

            var Tags = _dbContext.User.Where(_ => _.UserId == userId)
                .SelectMany(tl => tl.Posts.SelectMany(p => p.PostTags.Select(pt => pt.Tag)))
                .Select(_ => new { _.TagId, _.TagName }).Distinct().ToList();
            return Ok(Tags);
        }

    }

}