using Shopping.Ultilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Shopping.Contexts.Ultilities.Controllers
{
    [RoutePrefix("api/Domain")]
    public class DomainController : ApiController
    {

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            string domainName = HttpContext.Current.Request.Url.Host;

            DomainDto domainDto = new DomainDto();
            domainDto.Name = domainName;

            return Ok(domainDto);
        }
    }
}
