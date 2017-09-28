using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class SaleOffDto
    {
        public Guid Id { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Quantity { get; set; }
        public Guid? ProductId { get; set; }
        public ProductDto ProductDto { get; set; }

        public SaleOffDto(SaleOff saleOff, params object[] args)
        {
            Id = saleOff.Id;
            BeginDate = saleOff.BeginDate;
            EndDate = saleOff.EndDate;
            Quantity = saleOff.Quantity;
            ProductId = saleOff.ProductId;

            foreach(var arg in args)
            {
                if (arg is Product product)
                {
                    ProductDto = new ProductDto(product);
                }
            }
        }

        public SaleOff ToModel(SaleOff saleOff = null)
        {
            if (saleOff == null)
            {
                saleOff = new SaleOff();
                saleOff.Id = Guid.NewGuid();
            }

            saleOff.BeginDate = BeginDate;
            saleOff.EndDate = EndDate;
            saleOff.Quantity = Quantity;
            saleOff.ProductId = ProductId;

            return saleOff;
        }
    }
}