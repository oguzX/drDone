﻿using DrDone.Areas.Admin.ViewModels;
using DrDone.Infrastructure;
using DrDone.Models;
using NHibernate.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace DrDone.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("users")]
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        { 
            return View(new UsersIndex()
            {
                Users = Database.Session.Query<User>().ToList()
            });
        }

        public ActionResult New()
        {
            return View(new UsersNew
            {
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckBox
                {
                    Id = role.Id,
                    IsChecked = false,
                    Name = role.Name
                }).ToList()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(UsersNew form)
        {
            var user = new User();
            SyncRoles(form.Roles, user.Roles);

            if (Database.Session.Query<User>().Any(u => u.Username == form.Username))
                ModelState.AddModelError("username", "Bu kullancı adı daha önce kullanılmış");
            if (!ModelState.IsValid)
                return View(form);

            user.Email = form.Email;
            user.Name = form.Name;
            user.Surname = form.Surname;
            user.Phone = form.Phone;
            user.Username = form.Username;
            user.SetPassword(form.Password);

            Database.Session.Save(user);
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();
            return View(new UsersEdit
            {
                Username = user.Username,
                Email = user.Email,
                Roles = Database.Session.Query<Role>().Select(Role => new RoleCheckBox
                {
                    Id = Role.Id,
                    IsChecked = user.Roles.Contains(Role),
                    Name = Role.Name
                }).ToList()
            });
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsersEdit form)
        {
            var user = Database.Session.Load<User>(id);

            if (user == null)
                return HttpNotFound();

            SyncRoles(form.Roles, user.Roles);


            if (Database.Session.Query<User>().Any(u => u.Username == form.Username && u.Id != id))
                ModelState.AddModelError("username", "Kullanıcı Adı daha önce kullanılmış");
            if (!ModelState.IsValid)
                return View(form);


            user.Username = form.Username;
            user.Email = form.Email;
            
            Database.Session.Update(user);
            return RedirectToAction("index");

        }
        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();
            return View(new UsersResetPassword
            {
                Username = user.Username
            });
        }
        [HttpPost]
        public ActionResult ResetPassword(int id, UsersResetPassword form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();
            form.Username = user.Username;
            if (!ModelState.IsValid)
                return View(form);
            user.SetPassword(form.Password);
            Database.Session.Update(user);
            return RedirectToAction("index");

        }
        public ActionResult Delete(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();
            Database.Session.Delete(user);
            return RedirectToAction("index");
        }

        private void SyncRoles(IList<RoleCheckBox> checkboxes, IList<Role> roles)
        {
            var selectedRoles = new List<Role>();
            foreach(var role in Database.Session.Query<Role>())
            {
                var checkbox = checkboxes.Single(c => c.Id == role.Id);
                checkbox.Name = role.Name;
                if (checkbox.IsChecked)
                    selectedRoles.Add(role);
            }
            foreach (var toAdd in selectedRoles.Where(t => !roles.Contains(t)))
                roles.Add(toAdd);
            foreach (var toRomeve in roles.Where(t => !selectedRoles.Contains(t)).ToList())
                roles.Remove(toRomeve);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute(new { controller = "Home", action = "index" });
        }

    }
}