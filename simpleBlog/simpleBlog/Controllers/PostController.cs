using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using simpleBlog.Context;
using simpleBlog.ViewModels;
using simpleBlog.Models;
using simpleBlog.Infrastructure;
using System.Text.RegularExpressions;

namespace simpleBlog.Controllers
{
    public class PostController : Controller
    {
        simpleBolgContext context = new simpleBolgContext();
        private const int PerPage = 10;
        public ActionResult index(int page =1)
        {
            var postQuary = context.Posts
                .Include("Tags")
                .Include("User")
                .Where(p => p.DeletedAt == null)
                .OrderByDescending(p => p.CreatedAt);

            var totalPostCount = postQuary.Count();
            var postIds = postQuary.Skip((page - 1) * PerPage).Take(PerPage).Select(p => p.PostId).ToArray();
            var posts = postQuary.Where(p => postIds.Contains(p.PostId)).ToList();

            return View(new PostsIndex {
               Posts = new PageData<Post>(posts,totalPostCount,page,PerPage)
            });
        }

        public ActionResult Tag(string idAndSlug, int page = 1)
        {
            var parts = SeparateIdAndSlug(idAndSlug);
            if (parts == null)
            {
                return HttpNotFound();

            }
            var tag = context.Tags.FirstOrDefault(p => p.TagId == parts.Item1);
            if (tag == null)
            {
                return HttpNotFound();
            }
            if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToRoutePermanent("Post", new { id = parts.Item1, slug = tag.Slug });
            }


            var totalPostCount = tag.Posts.Count();
            var postIds = tag.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * PerPage)
                .Take(PerPage).Where(p => p.DeletedAt==null)
                .Select(p => p.PostId).ToArray();

            var posts = context.Posts
                .Include("Tags")
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => postIds.Contains(p.PostId))
                .ToList();
            return View(new PostsTag {

                Tag = tag,
                Posts = new PageData<Post>(posts,totalPostCount,page,PerPage)
            });

        }
        public ActionResult Show(string idAndSlug)
        {
            var parts = SeparateIdAndSlug(idAndSlug);
            if (parts == null)
            {
                return HttpNotFound();

            }
            var post  = context.Posts.FirstOrDefault(p => p.PostId == parts.Item1);
            if (post == null || post.IsDeleted)
            {
                return HttpNotFound();
            }
            if (!post.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToRoutePermanent("Post", new { id = parts.Item1, slug = post.Slug });
            }

            return View(new PostsShow
            {
                Post=post
            });
        }

        private Tuple<int, string> SeparateIdAndSlug(string idAndSlug)
        {
            var match = Regex.Match(idAndSlug, @"^(\d+)\-(.*)?$");
            if (!match.Success)
            {
                return null;
            }
            var id = int.Parse(match.Result("$1"));
            var slug = match.Result("$2");
            return Tuple.Create(id, slug);
        }
    }
}