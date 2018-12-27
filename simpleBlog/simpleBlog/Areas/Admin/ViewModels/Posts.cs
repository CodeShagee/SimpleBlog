using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using simpleBlog.Infrastructure;
using simpleBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace simpleBlog.Areas.Admin.ViewModels
{
    public class TagCheckbox
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
    public class PostsIndex
    {
        public PageData<Post> Posts { get; set; }
    }
    public class PostForm
    {
        public bool IsNew { get; set; }
        public int? PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public IList<TagCheckbox> Tags { get; set; }
    }
}