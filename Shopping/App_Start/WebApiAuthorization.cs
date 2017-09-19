using System.Web.Http;
using System.Web.Http.Controllers;

namespace Shopping.App_Start
{
    public class WebApiAuthorization : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return true;
        }
    }
}