using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrDone.Infrastructure;

namespace DrDone.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PostController : Controller
    {
        // GET: Admin/Post
        [SelectedTab("post")]
        public ActionResult Index()
        {
            return View("index");
        }
    }
}