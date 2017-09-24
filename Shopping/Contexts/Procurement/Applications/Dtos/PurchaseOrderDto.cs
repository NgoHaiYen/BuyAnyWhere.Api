using Shopping.Models;
using Shopping.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? BuyerId { get; set; }
        public Guid? ShopId { get; set; }
        public int? WorkFlowLevel { get; set; }
        public Constant.PurchaseOrderWorkFlowStatus WorkFlowStatus { get; set; }

        public PurchaseOrderDto(PurchaseOrder purchaseOrder)
        {
            Id = purchaseOrder.Id;
            Name = purchaseOrder.Name;
            Phone = purchaseOrder.Phone;
            CreatedDate = purchaseOrder.CreatedDate;
            BuyerId = purchaseOrder.BuyerId;
            ShopId = purchaseOrder.ShopId;
            WorkFlowStatus = (Constant.PurchaseOrderWorkFlowStatus)purchaseOrder.WorkFlowStatus;
            WorkFlowLevel = purchaseOrder.WorkFlowLevel;
        }

        public PurchaseOrder ToModel(PurchaseOrder purchaseOrder = null)
        {
            if (purchaseOrder == null)
            {
                purchaseOrder = new PurchaseOrder();
                purchaseOrder.Id = Guid.NewGuid();
            }

            purchaseOrder.Name = Name;
            purchaseOrder.Phone = Phone;
            purchaseOrder.CreatedDate = CreatedDate;
            purchaseOrder.BuyerId = BuyerId;
            purchaseOrder.ShopId = ShopId;
            purchaseOrder.WorkFlowStatus = (int)WorkFlowStatus;
            purchaseOrder.WorkFlowStatus = WorkFlowLevel;

            return purchaseOrder; 
        }
    }
}