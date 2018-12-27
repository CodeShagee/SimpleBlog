using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simpleBlog.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public Tag()
        {
            Posts = new List<Post>();
        }
    }
}