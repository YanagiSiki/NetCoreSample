using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NetCoreSample.Models;

namespace NetCoreSample.Components
{
    /// <summary>
    /// NavBar Component
    /// </summary>
    [ViewComponent(Name = "NavBar")]
    public class NavBarComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
                return View("LoginNavBar");
            return View("NotLoginNavBar");
        }
    }

    /// <summary>
    /// NavBar Component
    /// </summary>
    [ViewComponent(Name = "SideBar")]
    public class SideBarComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
                return View("LoginSideBar");
            return View("NotLoginSideBar");
        }
    }

    /// <summary>
    /// add badge of PostTags
    /// </summary>
    [ViewComponent(Name = "PostTags")]
    public class PostTags : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<PostTag> model)
        {
            if (model != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                var i = 0;
                model.ToList().ForEach(pt =>
                {
                    string str = $@"<span class='badge badge-pill badge-primary'>
                            <a href='#' >{pt.Tag.TagName}</a>
                            <i class='close fa fa-times'></i>
                            <input name='PostTags[{i}].Tag.TagId' value='{pt.Tag.TagId}'  type='hidden'/>
                        </span> ";
                    stringBuilder.Append(str);
                    i++;
                });
                return new HtmlContentViewComponentResult(new HtmlString(stringBuilder.ToString()));
            }
            return new HtmlContentViewComponentResult(new HtmlString(""));
        }
    }
}