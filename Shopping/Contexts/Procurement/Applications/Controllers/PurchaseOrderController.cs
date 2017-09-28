using Shopping.Models;
using System;
using System.Linq;
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

       [HttpGet]
       [Route("{purchaseOrderId}")]
       public IHttpActionResult Get([FromUri] Guid purchaseOrderId)
       {
            var purchaseOrder = shoppingEntities.PurchaseOrders.FirstOrDefault(t => t.Id == purchaseOrderId);
            if (purchaseOrder == null)
            {
                throw new BadRequestException("Không có PurchaseOrder");
            }

            var purchaseOrderDto = new PurchaseOrderDto(purchaseOrder, purchaseOrder.PurchaseOrderDetails, purchaseOrder.PurchaseOrderWorkFlows);

            return Ok(purchaseOrderDto);
       }

    }
}