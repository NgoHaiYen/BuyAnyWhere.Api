using Shopping.Contexts.Procurement.Applications.Dtos;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace Shopping.Contexts.Procurement.Applications.Controllers
{

    [RoutePrefix("api/Procurement/Products")]
    public class ProductController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public ProductController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] ProductFilterDto productFilterDto)
        {
            if (productFilterDto == null)
            {
                productFilterDto = new ProductFilterDto();
            }

            var products = productFilterDto.SkipAndTake(productFilterDto.ApplyTo(shoppingEntities.Products.Include(t => t.SaleOffs))).ToList();
            var productDtos = products.ConvertAll(t => new ProductDto(t, t.SaleOffs));

            return Ok(productDtos);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] ProductDto productDto)
        {
            var product = productDto.ToModel();
            shoppingEntities.Products.Add(product);

            return Get(product.Id);
        }

        [HttpGet]
        [Route("{productId}")]
        private IHttpActionResult Get([FromUri] Guid productId)
        {
            var product = shoppingEntities.Products.FirstOrDefault(t => t.Id == productId);
            if (product == null)
            {
                throw new BadRequestException("Không tồn tại Product");
            }

            var productDto = new ProductDto(product, product.SaleOffs);

            return Ok(productDto);
        }

    }
}
