using DrDone.Areas.Admin.ViewModels;
using DrDone.Models;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrDone.Areas.Admin.Controllers
{
    public class IndexController : Controller
    {
        // GET: Admin/Index
        public ActionResult Home()
        {
            return View(new IndexModel
            {
                UserCount = Database.Session.Query<User>().Count(),
                CategoryCount = Database.Session.Query<Category>().Count(),
                ProductCount = Database.Session.Query<Product>().Count()
            });
        }
    }
}