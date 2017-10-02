using Shopping.Contexts.Procurement.Applications.Dtos;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Shopping.Contexts.Procurement.Controllers.Applications
{
    [RoutePrefix("api/Procurement/Categories")]
    public class CategoryController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public CategoryController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] CategoryFilterDto categoryFilterDto)
        {
            if (categoryFilterDto == null)
            {
                categoryFilterDto = new CategoryFilterDto();
            }

            var categories = categoryFilterDto.SkipAndTake(categoryFilterDto.ApplyTo(shoppingEntities.Categories)).ToList();
            var categoryDtos = categories.ConvertAll(t => new CategoryDto(t));

            return Ok(categoryDtos);
        }


        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] CategoryDto categoryDto)
        {
            var category = categoryDto.ToModel();
            shoppingEntities.Categories.Add(category);
            shoppingEntities.SaveChanges();

            return Get(category.Id);
        }

        [HttpGet]
        [Route("{categoryId}")]
        private IHttpActionResult Get([FromUri] Guid categoryId)
        {
            var category = shoppingEntities.Categories.FirstOrDefault(t => t.Id == categoryId);
            if (category == null)
            {
                throw new BadRequestException("Không tồn tại Category");
            }

            var categoryDto = new CategoryDto(category);

            return Ok(categoryDto);
        }

    }
}