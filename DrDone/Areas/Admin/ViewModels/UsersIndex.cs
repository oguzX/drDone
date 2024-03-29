﻿using DrDone.Models;
using DrDone.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DrDone.Areas.Admin.ViewModels
{

    public class UsersIndex
    {
        public IEnumerable<User> Users { get; set; }

    }
    public class UsersNew:Signup
    {
        public IList<RoleCheckBox> Roles { get; set;}
    }
    public class UsersEdit
    {
        public IList<RoleCheckBox> Roles { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }
        [Required, MaxLength(128), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
    public class UsersResetPassword
    {
        [Required, MaxLength(128)]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RoleCheckBox
    {
        public int Id { get; set; }
        public bool IsChecked{ get; set; }
        public String Name { get; set; }
    }

}