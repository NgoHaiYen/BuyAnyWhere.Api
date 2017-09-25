using Shopping.Contexts.Procurement.Applications.Dtos;
using Shopping.Models;
using System.Linq;
using System.Web.Http;

namespace Shopping.Contexts.Procurement.Applications.Controllers
{
    [RoutePrefix("api/FavoriteCategory")]
    public class FavoriteCategoryController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public FavoriteCategoryController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var favoriteCategories = shoppingEntities.FavoriteCategories.ToList();
            var favoriteCategoryDtos = favoriteCategories.ConvertAll(t => new FavoriteCategoryDto(t));

            return Ok(favoriteCategoryDtos);
        }
    }
}
