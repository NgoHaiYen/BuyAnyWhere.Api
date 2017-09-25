using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using Shopping.Contexts.Procurement.Applications.Dtos;

namespace Shopping.Contexts.Procurement.Applications.Controllers
{

    [RoutePrefix("api/Procurement/PurchaseOrders")]
    public class PurchaseOrderController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public PurchaseOrderController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var purchaseOrders = shoppingEntities.PurchaseOrders.Include(t => t.PurchaseOrderDetails).Include(t => t.PurchaseOrderWorkFlows).ToList();

            var purchaseOrderDtos = purchaseOrders.ConvertAll(t => new PurchaseOrderDto(t, t.PurchaseOrderDetails, t.PurchaseOrderWorkFlows));

            return Ok(purchaseOrderDtos);
        }
    }
}