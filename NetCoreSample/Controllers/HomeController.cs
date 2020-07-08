using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Authorize;
using NetCoreSample.Models;
using NetCoreSample.Tools;

namespace NetCoreSample.Controllers
{
    //API Key ID: 4AIWnYxRQuCalJLR-hV26A
    //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
    //[AllowAnonymous]

    [Route("[controller]/[action]")]
    [Authorize(Roles.Admin)]
    [Authorize(AuthenticationSchemes = "CookieForView")]
    public class HomeController : BaseController
    {

        public HomeController(BaseContext dbContext) : base(dbContext)
        {

        }

        [AllowAnonymous]
        [Route("/Home/Index")]
        public IActionResult Index()
        {
            return Redirect("/");
        }

        [HttpGet]
        [IsNotLoginFilter]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (returnUrl.IsNotNull() && IsLocal(returnUrl))
                ViewBag.ReturnUrl = returnUrl;
            return View(new User());
        }

        [HttpGet]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieForView");
            await HttpContext.SignOutAsync("CookieForWebApi");
            // return RedirectToAction("index", "Home");
            return Redirect("/");
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [Route("~/")]
        [Route("/Home/Posts/{page?}")]
        [AllowAnonymous]
        public IActionResult Posts(int page)
        {
            page = page > 0 ? page : 1;
            var Posts = _dbContext.Post.OrderByDescending(_ => _.PostId).Pagination(page);
            var TotalPostCount = _dbContext.Post.Count();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((double)TotalPostCount / 5);
            // ViewBag.PageRange = 2;
            Posts.ForEach(_ =>
            {
                var tmp = _.PostContent.Split("<!--more-->\n").First();
                if (tmp.IsNotNull())_.PostContent = tmp;
            });
            return View(Posts);
        }

        [HttpGet("{postTitle?}")]
        [AllowAnonymous]
        public IActionResult Post(string postTitle)
        {
            if (postTitle.IsNullOrEmpty())throw new Exception("Page Not Found");
            int PostId;
            Post Post;

            if (int.TryParse(postTitle, out PostId))
            {
                Post = _dbContext.Post.FirstOrDefault(_ => _.PostId == PostId);
                if (Post == null)
                    throw new Exception("Post Not Found");
                return RedirectToAction("Post", new { postTitle = Post.PostTitle });
            }

            Post = _dbContext.Post.FirstOrDefault(_ => _.PostTitle == postTitle);
            if (Post == null)
                throw new Exception("Post Not Found");
            string UserId = HttpContext.User.Claims.FirstOrDefault(_ => _.Type == "UserId")?.Value;
            ViewBag.IsOwner = Post.UserId.ToString() == UserId;
            return View(Post);
        }

        [AllowAnonymous]
        public IActionResult Sucess()
        {
            SucessMessages.Add("新增文章成功");
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }

        // [HttpGet("{postId?}")]
        // [AllowAnonymous]
        // public IActionResult Post(int postId)
        // {
        //     if (postId == 0)throw new Exception("Page Not Found");
        //     var Post = _dbContext.Post.FirstOrDefault(_ => _.PostId == postId);
        //     if (Post == null)
        //         throw new Exception("Post Not Found");
        //     return RedirectToAction("Post", new { postTitle = Post.PostTitle });
        // }

        [Authorize(Roles.Admin)]
        [HttpGet("{postId?}")]
        public IActionResult Edit(int postId)
        {
            if (postId == 0)
            {
                return View(new Post()
                {
                    // PostId = 1,
                    PostTitle = "PostTitle",
                        PostContent = "# This is H1! \r\n PostContent",
                        //PostTags = PostTags
                });
            }

            var Post = _dbContext.Post.FirstOrDefault(_ => _.PostId == postId);
            if (Post == null)
                throw new Exception("Post Not Found");

            string UserId = HttpContext.User.Claims.FirstOrDefault(_ => _.Type == "UserId")?.Value;
            if (_dbContext.Post.AsNoTracking().FirstOrDefault(_ => _.PostId == postId)?.UserId.ToString() != UserId)
                throw new Exception("You are not owner !!");

            return View(Post);
        }

        [Route("/Home/Tags")]
        [AllowAnonymous]
        public IActionResult Tags()
        {
            return View();
        }

        [Route("/Home/Tag/{tagId?}/{page?}")]
        [AllowAnonymous]
        public IActionResult Tag(int tagId, int page)
        {
            if (tagId == 0 || _dbContext.Tag.All(_ => _.TagId != tagId))throw new Exception("Tag Not Found");
            page = page > 0 ? page : 1;
            ViewBag.Tag = _dbContext.Tag.Where(_ => _.TagId == tagId).FirstOrDefault();
            // ViewBag.PageRange = 2;            
            var queryPost = _dbContext.PostTag.Where(_ => _.TagId == tagId)
                .Select(posttags => posttags.Post)
                .OrderByDescending(_ => _.PostId);
            var TotalPostCount = queryPost.Count();
            var Posts = queryPost.Pagination(page).ToList();
            Posts.ForEach(_ =>
            {
                var tmp = _.PostContent.Split("<!--more-->\n").First();
                if (tmp.IsNotNull())_.PostContent = tmp;
            });
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((double)TotalPostCount / 5);
            return View(Posts);
        }

        //聽說只要增加webhook就可以在每次push完後，自動把code拉到伺服器，執行sh去deploey...?
        public IActionResult GitAutoPull()
        {
            //https://loune.net/2017/06/running-shell-bash-commands-in-net-core/
            var cmd = "";
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                }
            };
            process.Start();
            return Ok();
        }

        // https://docs.microsoft.com/zh-tw/aspnet/core/security/preventing-open-redirects?view=aspnetcore-2.2
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            { //如果是本地
                return Redirect(returnUrl);
            }

            throw new Exception("Page Not Found");
        }

        private bool IsLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl);
        }

        [AllowAnonymous]
        public IActionResult Test()
        {
            return View();
        }

        // [HttpGet]
        // [IsNotLoginFilter]
        // public IActionResult Register()
        // {
        //     return View(new User());
        // }
        // 
        // [HttpPost]
        // [IsNotLoginFilter]
        // public IActionResult Register(User User)
        // {
        //     // if (_dbContext.User.Where(u => u.Email == User.Email).Any())
        //     if (_dbContext.User.Where(u => u.Name == User.Name).Any())
        //     {
        //         WarningMessages.Add("Email已註冊過！");
        //         throw new Exception("Email已註冊過！");
        //     }
        //     User.Password = User.Password.HashPassword();
        //     // User.Active = false;
        //     // User.VerifyCode = StringTool.GenerateString(10);
        //     //SendGridHelper.SendEmailAsync();
        //     //SendGridHelper.SendVerifyCodeAsync(user.Email, user.Name, user.VerifyCode);

        //     _dbContext.User.Add(User);
        //     _dbContext.SaveChanges();
        //     SucessMessages.Add("成功註冊La！");
        //     return Redirect("/Home");
        // }

        // [HttpGet]
        // [IsNotLoginFilter]
        // public IActionResult VerifyAccount(string email, string userName, string code)
        // {
        //     var Dbusers = _dbContext.User.Where(u => u.Email == email && u.Name == userName);
        //     if (Dbusers.Any())
        //     {
        //         var Dbuser = Dbusers.First();
        //         if (Dbuser.Active)
        //         {
        //             WarningMessages.Add("已啟用");
        //             return Redirect("/Home");
        //         }
        //         Dbuser.Active = true;
        //         _dbContext.User.Update(Dbuser);
        //         _dbContext.SaveChanges();
        //         SucessMessages.Add("成功啟用");
        //         return Redirect("/Home");

        //     }
        //     throw new Exception("Code有誤");
        // }

    }
}