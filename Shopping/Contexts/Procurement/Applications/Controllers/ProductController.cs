using Shopping.Contexts.Procurement.Applications.Dtos;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Shopping.Contexts.Procurement.Applications.Interfaces;

namespace Shopping.Contexts.Procurement.Applications.Controllers
{

    [RoutePrefix("api/Procurement/Products")]
    public class ProductController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly INotificationService notificationService;

        public ProductController(ShoppingEntities shoppingEntities, INotificationService notificationService)
        {
            this.shoppingEntities = shoppingEntities;
            this.notificationService = notificationService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] ProductFilterDto productFilterDto)
        {
            if (productFilterDto == null)
            {
                productFilterDto = new ProductFilterDto();
            }

            var products = productFilterDto.SkipAndTake(productFilterDto
                .ApplyTo(shoppingEntities.Products.Include(t => t.SaleOffs).Where(t => t.Deleted == false))).ToList();
            var productDtos = products.ConvertAll(t => new ProductDto(t, t.SaleOffs));

            return Ok(productDtos);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] ProductDto productDto)
        {
            var product = productDto.ToModel();
            product.CreatedDate = System.DateTime.Now;
            product.Deleted = false;
            shoppingEntities.Products.Add(product);
            shoppingEntities.SaveChanges();


            // Notify favorite user
            var users = shoppingEntities.Categories.SelectMany(t => t.FavoriteCategories).Select(t => t.User).ToList();

            foreach(var user in users)
            {
                notificationService.Notify(product.Shop.Name + " đã thêm sản phẩm" + product.Name + "vào nhóm " + product.Category.Name, "", user.CloudToken);
            }

            return Ok(new ProductDto(product));
        }

        [HttpPut]
        [Route("{productId}")]
        public IHttpActionResult Put([FromUri] Guid productId, [FromBody] ProductDto productDto)
        {
            var product = shoppingEntities.Products.FirstOrDefault(t => t.Id == productId);

            if (product == null)
            {
                throw new BadRequestException("Product không tồn tại");
            }

            product.Name = productDto.Name;
            product.Quantity = productDto.Quantity;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.CategoryId = productDto.CategoryId;

            shoppingEntities.SaveChanges();

            return Ok(new ProductDto(product));
        }

        [HttpGet]
        [Route("{productId}")]
        public IHttpActionResult Get([FromUri] Guid productId)
        {
            var product = shoppingEntities.Products.FirstOrDefault(t => t.Id == productId);
            if (product == null)
            {
                throw new BadRequestException("Không tồn tại Product");
            }

            var productDto = new ProductDto(product, product.SaleOffs);

            return Ok(productDto);
        }

        [HttpDelete]
        [Route("{productId}")]
        public IHttpActionResult Delete([FromUri] Guid productId)
        {
            var product = shoppingEntities.Products.FirstOrDefault(t => t.Id == productId);
            if (product == null)
            {
                throw new BadRequestException("Không tồn tại Product");
            }

            product.Deleted = true;
            product.DetetedTime = System.DateTime.Now;
            shoppingEntities.SaveChanges();

            return Ok(new ProductDto(product));
        }
    }
}
