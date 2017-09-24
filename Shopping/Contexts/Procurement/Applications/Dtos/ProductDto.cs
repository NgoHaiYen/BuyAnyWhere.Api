using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? CategoryId { get; set; }

        public ProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Quantity = product.Quantity;
            Price = product.Price;
            Description = product.Description;
            CreatedDate = product.CreatedDate;
            ShopId = product.ShopId;
            CategoryId = product.CategoryId;
        }

        public Product ToModel(Product product = null)
        {
            if (product == null)
            {
                product = new Product();
                product.Id = Guid.NewGuid();
            }

            product.Name = Name;
            product.Quantity = Quantity;
            product.Price = Price;
            product.Description = Description;
            product.CreatedDate = CreatedDate;
            product.ShopId = ShopId;
            product.CategoryId = CategoryId;

            return product;
        }
    }
}