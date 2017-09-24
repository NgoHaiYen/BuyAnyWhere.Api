using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class PurchaseOrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public decimal? Quantity { get; set; }
        public Guid? ProductId { get; set; }

        public ProductDto ProductDto { get; set; }
        public PurchaseOrderDto PurchaseOrderDto { get; set; }

        public PurchaseOrderDetailDto(PurchaseOrderDetail purchaseOrderDetail, params object[] args)
        {
            Id = purchaseOrderDetail.Id;
            PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
            Quantity = purchaseOrderDetail.Quantity;
            ProductId = purchaseOrderDetail.ProductId;

            foreach (var arg in args)
            {
                if (arg is Product product)
                {
                    ProductDto = new ProductDto(product);
                }
                else if (arg is PurchaseOrder purchaseOrder)
                {
                    PurchaseOrderDto = new PurchaseOrderDto(purchaseOrder);
                }
            }
        }

        public PurchaseOrderDetail ToModel(PurchaseOrderDetail purchaseOrderDetail = null)
        {
            if (purchaseOrderDetail == null) {
                purchaseOrderDetail = new PurchaseOrderDetail();
                purchaseOrderDetail.Id = Guid.NewGuid();
            }

            purchaseOrderDetail.PurchaseOrderId = PurchaseOrderId;
            purchaseOrderDetail.Quantity = Quantity;
            purchaseOrderDetail.ProductId = ProductId;

            return purchaseOrderDetail;
        }
    }
}