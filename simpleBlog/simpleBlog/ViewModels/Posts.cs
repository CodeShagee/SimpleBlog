using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using simpleBlog.Infrastructure;
using simpleBlog.Models;

namespace simpleBlog.ViewModels
{
    public class PostsIndex
    {
        public PageData<Post> Posts { get; set; }
    }
    public class PostsShow
    {
        public Post Post { get; set; }
    }
    public class PostsTag
    {
        public Tag Tag { get; set; }
        public PageData<Post> Posts { get; set; }
    }
} 