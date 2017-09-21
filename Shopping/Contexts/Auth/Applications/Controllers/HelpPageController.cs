using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    public class HelpPageController : Controller
    {
        // GET: HelpPage
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }
}