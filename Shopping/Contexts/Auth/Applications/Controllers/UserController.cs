using System;
using System.Linq;
using System.Web.Http;
using Shopping.App_Start;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using Shopping.Ultilities;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/Users")]
    public class UserController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public UserController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] PaginateDto paginateDto)
        {
            if (paginateDto == null)
                paginateDto = new PaginateDto();

            var users = paginateDto.SkipAndTake(shoppingEntities.Users).ToList();
            var userDtos = users.ConvertAll(t => new UserDto(t, null, t.Role));

            return Ok(userDtos);
        }

        [HttpGet]
        [Route("{userId}")]
        public IHttpActionResult Get([FromUri] Guid userId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);

            if (user == null)
                throw new Exception("User not found!");

            var userDto = new UserDto(user, null, user.Role);
            return Ok(userDto);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] UserDto userDto)
        {
            var user = userDto.ToModel();
            shoppingEntities.Users.Add(userDto.ToModel());
            shoppingEntities.SaveChanges();

            return Get(user.Id);
        }

        [HttpPut]
        [Route("{userId}/Role/{roleId}")]
        public IHttpActionResult PutRole([FromUri] Guid userId, [FromUri] Guid roleId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            var role = shoppingEntities.Roles.FirstOrDefault(t => t.Id == roleId);

            if (user == null)
                throw new BadRequestException("ID người dùng không hợp lệ");

            user.Role = role ?? throw new BadRequestException("ID vai trò không hợp lệ");

            shoppingEntities.SaveChanges();
            return Get(userId);
        }
    }
}