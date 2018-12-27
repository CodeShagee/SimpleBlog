using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using simpleBlog.Controllers;

namespace simpleBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var nameSpace = new[] { typeof(PostController).Namespace };
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("RealPostRout", "post/{idAndSlug}", new { controller = "Post", action = "Show" }, nameSpace);
            routes.MapRoute("Post", "post/{id}-{slug}", new { controller = "Post", action = "Show" }, nameSpace);

            routes.MapRoute("RealTagRout", "tag/{idAndSlug}", new { controller = "Post", action = "Tag" }, nameSpace);
            routes.MapRoute("Tag", "tag/{id}-{slug}", new { controller = "Post", action = "Tag" }, nameSpace);

            routes.MapRoute("Home", "", new { Controller = "Post", Action = "index" },nameSpace);
            routes.MapRoute("Login", "login", new { Controller = "Auth", Action = "Login" },nameSpace);
            routes.MapRoute("Logout", "logout", new { Controller = "Auth", Action = "Logout" }, nameSpace);
            routes.MapRoute("Sidebar", "", new { controller = "Layout", action = "Sidebar" }, nameSpace);
        }
    }
}
