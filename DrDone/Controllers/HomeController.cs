using DrDone.Infrastructure;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace DrDone.Controllers
{
    public class HomeController : Controller
    {
        [SelectedTab("Home")]
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}