using Shopping.Models;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System;
using Shopping.Contexts.Procurement.Applications.Dtos;

namespace Shopping.Contexts.Procurement.Applications.Controllers
{
    [RoutePrefix("api/Procurement/Shops")]
    public class ShopController : ApiController
    {

        private readonly ShoppingEntities shoppingEntities;

        public ShopController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var shops = shoppingEntities.Shops.ToList();
            var shopDtos = shops.ConvertAll(t => new ShopDto(t));

            return Ok(shopDtos);
        }

        [HttpGet]
        [Route("{shopId}")]
        public IHttpActionResult Get([FromUri] Guid shopId)
        {
            var shop = shoppingEntities.Shops.FirstOrDefault(t => t.Id == shopId);
            if (shop == null)
            {
                throw new BadRequestException("Không tìm thấy Shop");
            }

            var shopDto = new ShopDto(shop);

            return Ok(shopDto);
        }


        [HttpGet]
        [Route("{shopId}/Products/All")]
        public IHttpActionResult GetAllProducts([FromUri] Guid shopId, [FromUri] ProductFilterDto productFilterDto)
        {
            var products = productFilterDto.SkipAndTake(productFilterDto
                .ApplyTo( shoppingEntities.Products.Where(t => t.ShopId == shopId)))
                .ToList();
            var productDtos = products.ConvertAll(t => new ProductDto(t));
            return Ok(productDtos);
        }


        [HttpGet]
        [Route("{shopId}/Products")]
        public IHttpActionResult GetProducts([FromUri] Guid shopId, [FromUri] ProductFilterDto productFilterDto)
        {
            var products = productFilterDto.SkipAndTake(productFilterDto
                .ApplyTo(shoppingEntities.Products.Where(t => t.ShopId == shopId && t.Deleted == false)))
                .ToList();
            var productDtos = products.ConvertAll(t => new ProductDto(t));
            return Ok(productDtos);
        }

        [HttpGet]
        [Route("{shopId}/Products/Deleted")]
        public IHttpActionResult GetDeletedProducts([FromUri] Guid shopId, [FromUri] ProductFilterDto productFilterDto)
        {
            var products = productFilterDto.SkipAndTake(productFilterDto
                .ApplyTo(shoppingEntities.Products.Where(t => t.ShopId == shopId && t.Deleted == true)))
                .ToList();
            var productDtos = products.ConvertAll(t => new ProductDto(t));
            return Ok(productDtos);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] ShopDto shopDto)
        {
            var shop = shopDto.ToModel();

            shoppingEntities.Shops.Add(shop);
            shoppingEntities.SaveChanges();

            return Ok(new ShopDto(shop));
        }

        [HttpPut]
        [Route("{shopId}")]
        public IHttpActionResult Put([FromUri] Guid shopId, [FromBody] ShopDto shopDto)
        {
            Shop shop = shoppingEntities.Shops.FirstOrDefault(t => t.Id == shopId);

            if (shop == null)
            {
                throw new BadRequestException("Không tìm thấy Shop");
            }

            shopDto.ToModel(shop);
            shoppingEntities.SaveChanges();

            return Ok(new ShopDto(shop));
        }
    }
}