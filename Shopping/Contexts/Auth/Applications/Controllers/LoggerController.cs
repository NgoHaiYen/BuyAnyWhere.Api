using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Logger")]
    public class LoggerController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public LoggerController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(shoppingEntities.Loggers.OrderBy(t => t.DateTime).ToList());
        }
    }
}
