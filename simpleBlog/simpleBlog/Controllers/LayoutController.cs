using simpleBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using simpleBlog.Context;

namespace simpleBlog.Controllers
{
    public class LayoutController : Controller
    {
        simpleBolgContext context = new simpleBolgContext();
        // GET: Layout
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var uname = User.Identity != null ? User.Identity.Name : "";
            var Islog = User.Identity != null;
            var tags = context.Tags.Select(t => new
            {
                t.TagId,
                t.Name,
                t.Slug,
                PostCount = t.Posts.Count()
            }).Where(t => t.PostCount > 0).OrderByDescending(p => p.PostCount).ToList()
                .Select(t => new SidebarTag(t.TagId, t.Name, t.Slug, t.PostCount));

            return View(new LayoutSidebar {
                IsLoggedIn =Islog,
                username =uname,
                isAdmin = User.IsInRole("admin"),
                Tags= tags
            });
        }
    }
}