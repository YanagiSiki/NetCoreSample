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

    
}