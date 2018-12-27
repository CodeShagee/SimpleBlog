using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using simpleBlog.Context;
using simpleBlog.Areas.Admin.ViewModels;
using simpleBlog.Infrastructure;
using simpleBlog.Models;
using System.Data.Entity;
using System.Web.Security;
using simpleBlog.Infrastructure.Extention;

namespace simpleBlog.Areas.Admin.Controllers
{
    [Authorize (Roles ="admin")]
    public class PostController : Controller
    {
        simpleBolgContext context = new simpleBolgContext();
        private const int PerPage = 10;
        // GET: Admin/Admin
        public ActionResult Index(int page =1)
        {
            var totalPostCount = context.Posts.Count();

            var currntPosts = context.Posts
                .Include("User")
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * PerPage)
                .Take(PerPage)
                .ToList();

            return View(new PostsIndex {

                Posts = new PageData<Post>(currntPosts,totalPostCount,page,PerPage)

            });
        }
        public ActionResult New()
        {
            return View("Form", new PostForm {
                IsNew = true,
                Tags = context.Tags.Select(t => new TagCheckbox
                {
                    Id = t.TagId,
                    Name = t.Name,
                    IsChecked = false
                }).ToList()
            });
            
        }
        public ActionResult Edit(int id)
        {
           var post = context.Posts.FirstOrDefault(u => u.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }
            var list = post.Tags.Select(t => t.TagId).ToList();
            return View("Form", new PostForm
            {
                IsNew=false,
                PostId=id,
                Title = post.Title,
                Slug = post.Slug,
                Content = post.Content,
                Tags = context.Tags.Select(t => new TagCheckbox
                {
                    Id = t.TagId,
                    Name = t.Name,
                    IsChecked= list.Contains(t.TagId)
                }).ToList()
                

            });
        }
        [HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult Form(PostForm form)
        {
            form.IsNew = form.PostId == null;

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var selectedTags = ReconsileTags(form.Tags).ToList() ;
            Post post;
            if (form.IsNew)
            {
                
                var crntUser = context.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                post = new Post
                {
                    Title = form.Title,
                    Slug = form.Slug,
                    Content = form.Content,
                    CreatedAt = DateTime.Now,
                    User = crntUser
                };
                foreach(var tag in selectedTags)
                {
                    post.Tags.Add(tag);
                }

                context.Posts.Add(post);
            }else
            {
                post = context.Posts.FirstOrDefault(u => u.PostId == form.PostId);

                if(post == null)
                {
                    return HttpNotFound();
                }
                post.Title = form.Title;
                post.Slug = form.Slug;
                post.Content = form.Content;
                post.UpdatedAt = DateTime.Now;

                foreach(var toAdd in selectedTags.Where(t => !post.Tags.Contains(t)))
                {
                    post.Tags.Add(toAdd);
                }
                foreach(var toRemove in post.Tags.Where(t => !selectedTags.Contains(t)).ToList())
                {
                    post.Tags.Remove(toRemove);
                }

                context.Posts.Attach(post);
                context.Entry(post).State =EntityState.Modified;
            }

            context.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Trash(int id)
        {
            var post = context.Posts.FirstOrDefault(u => u.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }
            
            post.DeletedAt = DateTime.Now;
            context.Posts.Attach(post);
            context.Entry(post).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Restore(int id)
        {
            var post = context.Posts.FirstOrDefault(u => u.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }

            post.DeletedAt = null;
            context.Posts.Attach(post);
            context.Entry(post).State = EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var post = context.Posts.FirstOrDefault(u => u.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }
            context.Posts.Remove(post);
            context.SaveChanges();

            return RedirectToAction("index");
        }

        private IEnumerable<Tag> ReconsileTags(IEnumerable<TagCheckbox> tags)
        {
            foreach(var tag in tags.Where(t => t.IsChecked))
            {
                if(tag.Id != null)
                {
                    yield return context.Tags.FirstOrDefault(t => t.TagId == tag.Id);
                    continue;
                }

                var exsistingTag = context.Tags.FirstOrDefault(t => t.Name == tag.Name);
                if(exsistingTag != null)
                {
                    yield return exsistingTag;
                    continue;
                }

                var newTag = new Tag
                {
                    Name = tag.Name,
                    Slug = tag.Name.Slugify()
                };

                context.Tags.Add(newTag);
                context.SaveChanges();
                yield return newTag;
            }
        }
    }
}