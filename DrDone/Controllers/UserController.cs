using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DrDone.Models;
using DrDone.ViewModels;
using NHibernate.Linq;

namespace DrDone.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View("login");
            return RedirectToRoute(new { controller = "Home", action = "index" });
        }

        public ActionResult Signup()
        {
            return View(new Signup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(Signup form)
        {
            var user = new User();

            if (Database.Session.Query<User>().Any(u => u.Username == form.Username))
                ModelState.AddModelError("username", "Bu kullancı adı daha önce kullanılmış");
            if (!ModelState.IsValid)
                return View(form);

            user.Email = form.Email;
            user.Name = form.Name;
            user.Surname = form.Surname;
            user.Phone = form.Phone;
            user.Username = form.Username;

            user.Roles = Database.Session.Query<Role>().Where(x => x.Name == "User").ToList();
            user.SetPassword(form.Password);

            Database.Session.Save(user);
            return RedirectToAction("login",new { signup = true});
        }
        public ActionResult Login(bool signup = false)
        {
            if(!User.Identity.IsAuthenticated)
                return View(new AuthLogin { isSignup = signup});
            return RedirectToRoute("~/"+ Request.QueryString["ReturnUrl"]);
        }

        [HttpPost]
        public ActionResult Login(AuthLogin form, string returnUrl)
        {
            form.isSignup = false;
            var user = Database.Session.Query<User>().FirstOrDefault(u=> u.Username == form.Username);
            if (user == null)
                DrDone.Models.User.FakeHash();
            if (user == null || !user.CheckPassword(form.Password))
                ModelState.AddModelError("Username","Kullanıcı adı veya şifre yanlış");
            if (!ModelState.IsValid)
                return View("login", form);
            FormsAuthentication.SetAuthCookie(user.Username,true);
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);
            return RedirectToRoute(new { controller="Home",action="index"});
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute(new { controller = "Home", action = "index" });
        }
    }
}