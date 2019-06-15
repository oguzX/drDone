using DrDone.Infrastructure;
using DrDone.Models;
using DrDone.ViewModels;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace DrDone.Controllers
{
    public class HomeController : Controller
    {
        [SelectedTab("Home")]
        public ActionResult Index()
        {
            var Starred = Database.Session.Query<Product>().OrderByDescending(x => x.UpdatedAt);
            if (Starred.Where(p => p.Status == 2).ToList().Count > 0)
            {
                return View(new IndexModel()
                {
                    Categories = Database.Session.Query<Category>().ToList(),
                    StaredProduct = Starred.Where(p => p.Status == 2).ToList()
            });
            }
            return View(new IndexModel()
            {
                Categories = Database.Session.Query<Category>().ToList(),
                StaredProduct = Starred.ToList()
            });
        }
        public ActionResult Category(int id=-1)
        {
            if (id == null || id == -1)
                return RedirectToRoute(new { controller = "Home", action = "category" });
            var Cat = Database.Session.Load<Category>(id);
            var _products = Database.Session.Query<Product>()
                .OrderByDescending(c => c.CreatedAt);

            List<Product> products = new List<Product>();
            foreach (var pro in _products)
            {
                foreach (var cat in pro.Category)
                {
                    if (cat.Id == id)
                    {
                        products.Add(pro);
                        break;
                    }
                }
            }
            return View("Category", new StoreModel()
            {
                Name = Cat.Name,
                Categories = Database.Session.Query<Category>().ToList(),
                Products = products
            });
        }

    }
}