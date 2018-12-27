using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using simpleBlog.ViewModels;
using System.Web.Security;
using simpleBlog.Context;
using simpleBlog.Infrastructure;

namespace simpleBlog.Controllers
{
    public class AuthController : Controller
    {
        simpleBolgContext context = new simpleBolgContext();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AuthLogin form ,string returnUrl)
        {
            var user = context.Users.FirstOrDefault(u => u.Username == form.Username);
            if(user == null || (user.PasswordHash != new Hashing().getHashedPw(form.Password)))
            {
                ModelState.AddModelError("Username", "Username or Password is incorrect");
            }

            if(!ModelState.IsValid)
            return View(form);

            
            FormsAuthentication.SetAuthCookie(user.Username, true);

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToRoute("home");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("home");
        }
    }
}