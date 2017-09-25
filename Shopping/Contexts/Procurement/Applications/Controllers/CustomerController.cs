using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Shopping.Contexts.Procurement.Applications.Dtos;

namespace Shopping.Contexts.Procurement.Applications.Controllers
{

    [RoutePrefix("api/Procurement/Users")]
    public class CustomerController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public CustomerController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpPost]
        [Route("{userId}/FavoriteCategories/{categoryId}")]
        public IHttpActionResult PostFavoriteCategory([FromUri] Guid userId, [FromUri] Guid categoryId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            var category = shoppingEntities.Categories.FirstOrDefault(t => t.Id == categoryId);

            if (user == null)
            {
                throw new BadRequestException("User khong ton tai");
            }

            if (category == null)
            {
                throw new BadRequestException("Category khong ton tai");
            }

            FavoriteCategory favoriteCategory = new FavoriteCategory();
            favoriteCategory.Id = Guid.NewGuid();
            favoriteCategory.Category = category;
            favoriteCategory.User = user;

            shoppingEntities.SaveChanges();
            return Ok(favoriteCategory);
        }

        [HttpDelete]
        [Route("{userId}/FavoriteCategories/{categoryId}")]
        public IHttpActionResult DeleteFavoriteCategory([FromUri] Guid userId, [FromUri] Guid categoryId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            var category = shoppingEntities.Categories.FirstOrDefault(t => t.Id == categoryId);

            if (user == null)
            {
                throw new BadRequestException("User khong ton tai");
            }

            if (category == null)
            {
                throw new BadRequestException("Category khong ton tai");
            }

            FavoriteCategory favoriteCategory = shoppingEntities.FavoriteCategories.FirstOrDefault(t => t.UserId == userId && t.CategoryId == categoryId);

            if (favoriteCategory == null)
            {
                throw new BadRequestException("User chua thich Category nay");
            }

            shoppingEntities.FavoriteCategories.Remove(favoriteCategory);
            shoppingEntities.SaveChanges();

            return Ok(favoriteCategory);
        }

        [HttpGet]
        [Route("{userId}/FavoriteCategories")]
        public IHttpActionResult GetFavoriteCategories([FromUri] Guid userId)
        {
            var users = shoppingEntities.Users.Include(t => t.FavoriteCategories.Select(u => u.Category)).FirstOrDefault(t => t.Id == userId);

            var favoriteCategories = users.FavoriteCategories.Select(t => t.Category).ToList();

            var favoriteCategoryDtos = favoriteCategories.ConvertAll(t => new CategoryDto(t));

            return Ok(favoriteCategoryDtos);
        }
    }
}
