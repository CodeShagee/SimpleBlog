using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using simpleBlog.Models;
using System.ComponentModel.DataAnnotations;

namespace simpleBlog.Areas.Admin.ViewModels
{
    public class RoleCheckbox
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }
    public class UserIndex
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class UserNew
    {
        public IList<RoleCheckbox> Roles { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Required,MaxLength(256),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class UserEdit
    {
        public IList<RoleCheckbox> Roles { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class UserResetPassword
    {
       
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}