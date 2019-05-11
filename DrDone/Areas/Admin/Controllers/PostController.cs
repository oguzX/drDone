using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrDone.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        [Authorize(Roles ="admin")]
        // GET: Admin/Post
        public ActionResult Index()
        {
            return View("index");
        }
    }
}