using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace simpleBlog.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        [NotMapped]
        public bool IsDeleted { get { return DeletedAt != null; } }

        public virtual ICollection<Tag> Tags { get; set; }

        public Post()
        {
            Tags = new List<Tag>();
        }
    }
}