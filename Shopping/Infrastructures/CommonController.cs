using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shopping.Infrastructures
{
    public class CommonController : ApiController
    {

        public IHttpActionResult Return(object obj)
        {
            return Json(obj);
        }
    }
}
