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

            return Ok(shops);
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

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] ShopDto shopDto)
        {
            var shop = shopDto.ToModel();

            shoppingEntities.Shops.Add(shop);
            shoppingEntities.SaveChanges();

            return Get(shop.Id);
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

            return Get(shop.Id);
        }
    }
}