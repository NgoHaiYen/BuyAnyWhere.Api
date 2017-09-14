using System.Web.Http;

namespace Shopping.Contexts.Auths.Infrastructures
{
    public class CommonController : ApiController
    {
        public IHttpActionResult Return(object obj)
        {
            return Json(obj);
        }
    }
}
