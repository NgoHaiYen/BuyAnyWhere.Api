using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Shopping.Contexts.Procurement.Infrastructures
{
    public class PurchaseOrderRepository
    {
        private readonly ShoppingEntities shoppingEntities;

        public PurchaseOrderRepository(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        public List<PurchaseOrder> Get()
        {
            var purchaseOrders = shoppingEntities.PurchaseOrders.Include(t => t.PurchaseOrderDetails)
                .Include(t => t.PurchaseOrderWorkFlows).ToList();

            return purchaseOrders;
        }


        //public bool Reject0(Guid purchaseOrderId)
        //{
        //    var purchaseOrder = shoppingEntities.PurchaseOrders.Include(t => t.PurchaseOrderDetails)
        //       .Include(t => t.PurchaseOrderWorkFlows).FirstOrDefault(t => t.Id == purchaseOrderId);

        //    purchaseOrder.

        //    return true;
        //}
    }
}