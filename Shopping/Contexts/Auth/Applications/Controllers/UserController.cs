using System;
using System.Linq;
using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using System.Data.Entity;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System.Web;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/Users")]
    public class UserController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        private readonly IUltilityService ultilityService;

        public UserController(ShoppingEntities shoppingEntities, IUltilityService ultilityService)
        {
            this.shoppingEntities = shoppingEntities;
            this.ultilityService = ultilityService;
        }
        
        [HttpPut]
        [Route("current/CloundToken")]
        public IHttpActionResult PostCloudToken([FromBody] string cloudToken)
        {
            var token = ultilityService.GetHeaderToken(HttpContext.Current);
            var user = ultilityService.GetUserFromToken(token);
            user.CloudToken = cloudToken;
            shoppingEntities.Entry<User>(user).State = EntityState.Modified;
            shoppingEntities.SaveChanges();
            return Ok(new UserDto(user));
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync([FromUri] UserFilterDto userFilterDto)
        {
            if (userFilterDto == null)
            {
                userFilterDto = new UserFilterDto();
            }

            var users = await userFilterDto.SkipAndTake(userFilterDto.ApplyTo(shoppingEntities.Users.AsNoTracking().Include(t => t.Role))).ToListAsync();
            var userDtos = users.ConvertAll(t => new UserDto(t, t.Role));

            return Ok(userDtos);
        }



        [HttpGet]
        [Route("{userId}")]
        public IHttpActionResult Get([FromUri] Guid userId)
        {
            var user = shoppingEntities.Users.Include(t => t.Role).FirstOrDefault(t => t.Id == userId);

            if (user == null)
                throw new BadRequestException("Không tìm thấy User này");

            var userDto = new UserDto(user);

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


        [HttpGet]
        [Route("Counter")]
        public IHttpActionResult Count([FromUri] UserFilterDto userFilterDto)
        {
            if (userFilterDto == null)
            {
                userFilterDto = new UserFilterDto();
            }

            var count = userFilterDto.ApplyTo(shoppingEntities.Users).Count();
            return Ok(count);
        }



        [HttpPut]
        [Route("{userId}/Roles/{roleId}")]
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