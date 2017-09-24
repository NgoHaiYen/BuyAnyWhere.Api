using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Shopping.Contexts.Procurement.Controllers.Applications
{
    [RoutePrefix("api/Procurement/Categories")]
    public class CategoryController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public CategoryController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        public IHttpActionResult Get()
        {

        }
    }
}