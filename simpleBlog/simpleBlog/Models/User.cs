using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simpleBlog.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public User()
        {
            Roles = new List<Role>();
        }
    }
}