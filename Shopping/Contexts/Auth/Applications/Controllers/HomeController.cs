using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            return Redirect("~/swagger");
        }
    }
}
