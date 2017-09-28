using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class PurchaseOrderWorkFlowDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public int? Level { get; set; }
        public int? Status { get; set; }
        public string Reason { get; set; }

        public PurchaseOrderWorkFlowDto(PurchaseOrderWorkFlow purchaseOrderWorkFlow)
        {
            Id = purchaseOrderWorkFlow.Id;
            UserId = purchaseOrderWorkFlow.UserId;
            Level = purchaseOrderWorkFlow.Level;
            Status = purchaseOrderWorkFlow.Status;
            Reason = purchaseOrderWorkFlow.Reason;
        }

        public PurchaseOrderWorkFlow ToModel(PurchaseOrderWorkFlow purchaseOrderWorkFlow = null)
        {
            if (purchaseOrderWorkFlow == null)
            {
                purchaseOrderWorkFlow = new PurchaseOrderWorkFlow();
                purchaseOrderWorkFlow.Id = Guid.NewGuid();
            }

            purchaseOrderWorkFlow.UserId = UserId;
            purchaseOrderWorkFlow.Level = Level;
            purchaseOrderWorkFlow.Status = Status;
            purchaseOrderWorkFlow.Reason = Reason;

            return purchaseOrderWorkFlow;
        }
    }
}