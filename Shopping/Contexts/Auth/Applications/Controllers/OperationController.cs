using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/Apis")]
    public class OperationController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public OperationController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] ApiFilterDto apiFilterDto)
        {
            if (apiFilterDto == null)
            {
                apiFilterDto = new ApiFilterDto();
            }

            var apis = apiFilterDto.SkipAndTake(apiFilterDto.ApplyTo(shoppingEntities.Apis.Include(t => t.Roles))).ToList();
            var apiDtos = apis.Select(t => new ApiDto(t, t.Roles)).ToList();

            return Ok(apiDtos);
        }
    }
}