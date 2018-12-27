using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using simpleBlog.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace simpleBlog.Context
{
    public class simpleBolgContext : DbContext
    {
        //public simpleBolgContext(): base("SimpleBlogDB")
        //{
        //}
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }

    }
}