using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;


            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
