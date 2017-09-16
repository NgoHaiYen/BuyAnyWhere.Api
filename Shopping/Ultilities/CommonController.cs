using System.Web.Http;

namespace Shopping.Ultilities
{
    public class CommonController : ApiController
    {
        public IHttpActionResult Return(object obj)
        {
            // Logging here 
            return Ok(obj);
        }
    }
}
