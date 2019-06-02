using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DrDone.Models;
using DrDone.ViewModels;

namespace DrDone.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AuthLogin form, string returnUrl)
        {
            var user = Database.Session.Query<User>().FirstOrDefault(u=> u.Username == form.Username);
            if (user == null)
                DrDone.Models.User.FakeHash();
            if (user == null || !user.CheckPassword(form.Password))
                ModelState.AddModelError("Username","Kullanıcı adı veya şifre yanlış");
            if (!ModelState.IsValid)
                return View(form);
            FormsAuthentication.SetAuthCookie(user.Username,true);
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);
            return RedirectToRoute("home");
        }
    }
}