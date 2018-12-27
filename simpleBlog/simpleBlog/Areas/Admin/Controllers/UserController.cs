using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using simpleBlog.Context;
using simpleBlog.Models;
using simpleBlog.Areas.Admin.ViewModels;
using System.Data.Entity;
using simpleBlog.Infrastructure;

namespace simpleBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        // GET: Admin/User
        simpleBolgContext context = new simpleBolgContext();
        public ActionResult Index()
        {
            
            UserIndex userOb = new UserIndex();

            userOb.Users = context.Users.ToList();

            return View(userOb);
        }
        public ActionResult New()
        {
            return View(new UserNew
            {
                Roles = context.Roles.Select(r => new RoleCheckbox
                {
                    Id = r.RoleId,
                    IsChecked = false,
                    Name = r.RoleName
                }).ToList()
            });
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult New( UserNew form)
        {
            var user = new User();
             SyncRoles(form.Roles, user.Roles);


            if (context.Users.Any(u => u.Username == form.Username)) {
                ModelState.AddModelError("Username", "Username must be unique");
            }
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            user.Username = form.Username;
               user.Email = form.Email;
                user.PasswordHash = this.getHashedPw(form.Password);
            
            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("index");
        }

        

        public ActionResult Edit(int id)
        {
            var user = context.Users.First(i => i.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var list = user.Roles.Select(x => x.RoleId);
            return View(new UserEdit {
                Username = user.Username,
                Email = user.Email,
                Roles = context.Roles.Select(r => new RoleCheckbox
                {
                    Id = r.RoleId,
                    IsChecked = list.Contains(r.RoleId),
                    Name = r.RoleName
                }).ToList()
            });

        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserEdit form)
        {
            var user = context.Users.First(i => i.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            SyncRoles(form.Roles, user.Roles);
            if (context.Users.Any(u => u.Username == form.Username && u.UserID!= id))
            {
                ModelState.AddModelError("Username", "Username must be unique");
            }
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            user.Username = form.Username;
            user.Email = form.Email;

            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            
            context.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult ResetPassword(int id)
        {
            var user = context.Users.First(i => i.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new UserResetPassword
            {
                Username = user.Username
               });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id, UserResetPassword form)
        {
            var user = context.Users.First(i => i.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            form.Username = user.Username;
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            user.PasswordHash = this.getHashedPw(form.Password);
            
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;

            context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = context.Users.First(i => i.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("index");
        }
        public string getHashedPw(string password)
        {
            Hashing hash = new Hashing();
            return hash.getHashedPw(password);
        }
        private void SyncRoles(IList<RoleCheckbox> checkboxes, ICollection<Role> roles)
        {
            var selectedRoles = new List<Role>();
            foreach(var role in context.Roles.ToList())
            {
                var checkboxe = checkboxes.Single(c => c.Id == role.RoleId);
                checkboxe.Name = role.RoleName;

                if (checkboxe.IsChecked)
                {
                    selectedRoles.Add(role);
                }
            }
            foreach(var toAdd in selectedRoles.Where(a => !roles.Contains(a)))
            {
                roles.Add(toAdd);
            }
            foreach(var toRemove in roles.Where(a => !selectedRoles.Contains(a)).ToList())
            {
                roles.Remove(toRemove);
            }
        }
    }
}